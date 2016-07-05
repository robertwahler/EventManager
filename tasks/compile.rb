module BasicUnity

  class Compile < Thor
    namespace :compile
    include Thor::Actions
    include BasicUnity::BasicUnityHelper

    # adds :quiet, :skip, :pretent, :force
    add_runtime_options!

    # used for syntax checking and building dll that can be run through its own unit tests
    desc "mcs", "create .mcs style response file from solution that includes editor and non-editor code and references"
    def mcs
      dll =  File.join(ROOT_FOLDER, "tmp", "EventManager.dll")
      output =  File.join(ROOT_FOLDER, "tmp", "EventManager.mcs")

      # ensure the tmp folder exists
      FileUtils::mkdir 'tmp' unless File.exists?('tmp')

      lines = []

      # header
      lines << "-debug"
      lines << "-target:library"
      lines << "-nowarn:0169"
      lines << "-out:'#{dll}'"

      # parse main sln first pass
      lines += parse_csproj(File.join(ROOT_FOLDER, "Assembly-CSharp-firstpass.csproj"))

      # parse editor sln first pass
      lines += parse_csproj(File.join(ROOT_FOLDER, "Assembly-CSharp-Editor-firstpass.csproj"))

      # parse editor sln
      lines += parse_csproj(File.join(ROOT_FOLDER, "Assembly-CSharp-Editor.csproj"))

      # parse main sln
      lines += parse_csproj(File.join(ROOT_FOLDER, "Assembly-CSharp.csproj"))

      # remove dupe assemblies, if any (tvOS, iOS)
      cleaned_lines = []
      dupes = []
      lines.each do |line|
        if line.match(/^\-r/)
          assembly_name = line.gsub(/.*\//, "")
          if dupes.include?(assembly_name)
            next
          else
            dupes << assembly_name
          end
        end
        cleaned_lines << line
      end

      # remove any dupe lines (mainly defines)
      lines = cleaned_lines.clone
      cleaned_lines = []
      processed = []
      lines.each do |line|
        if processed.include?(line)
          next
        else
          processed << line
        end
        cleaned_lines << line
      end

      # write .mcs file
      File.open(output, 'w') do |file|
        file.write(cleaned_lines.join("\n"))
      end

    end

    desc "test", "compile nunit tests"
    def test
      command = "#{mcs_binary} \
                -debug \
                -recurse:'Assets/EventManager/Source/*.cs' \
                -recurse:'Assets/EventManager/Test/Editor/*.cs' \
                -sdk:2 \
                -target:library \
                -lib:/Applications/Unity/Unity.app/Contents/Frameworks/Managed/ \
                -r:UnityEngine \
                -r:UnityEditor \
                -r:Mono.Cecil \
                -r:Mono.Cecil.Mdb \
                -lib:/Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/GUISystem/ \
                -r:UnityEngine.UI \
                -lib:/Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/EditorTestsRunner/Editor/ \
                -r:nunit.framework \
                -out:tmp/EventManager.Tests.dll"
      run(command)
    end

    private

    def parse_csproj(filename)
      lines = []

      if File.exists?(filename)
        File.open(filename, "r") do |file|
          doc = Nokogiri::XML(file)

          # source files
          doc.css('Compile').each do |element|
            name = element.attr("Include")
            name = name.gsub(/\\/, "/") if posix?
            say_status "SOURCE", name, :green
            lines << "'#{name}'"
          end

          # explicitly hinted dll paths
          doc.css('HintPath').each do |element|
            name = element.children.text.chomp
            say_status "DLL", name, :green
            lines << "-r:'#{name}'"
          end

          # compiler defines
          doc.css('DefineConstants').each do |element|
            name = element.children.text.chomp
            name.split(/;/).each do |define|
              say_status "DEFINE", define, :green
              lines << "-define:'#{define}'"
            end
          end
        end
      end

      lines
    end

    def mcs_binary
      unity_root="/Applications/Unity/Unity.app/Contents/Frameworks"
      "#{unity_root}/MonoBleedingEdge/bin/mcs"
    end

    # where to start looking for templates, required by the template methods
    # even though we are using absolute paths
    def self.source_root
      File.dirname(__FILE__)
    end

    def set_instance_variables
      @product = options[:product] ? options[:product]: default_product
      raise "invalid product: #{@product}" unless valid_products.include?(@product)

      @stage = options[:development] ? "development" : default_stage
    end
  end
end

