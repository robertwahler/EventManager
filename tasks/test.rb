require 'nokogiri'
require 'fileutils'

module BasicUnity

  class Test < Thor
    namespace :test
    include Thor::Actions
    include BasicUnity::BasicUnityHelper

    # adds :quiet, :skip, :pretent, :force
    add_runtime_options!

    # Run unit tests
    # https://docs.unity3d.com/Manual/testing-editortestsrunner.html
    # 0 = succeeded, 2 = succeeded, some tests failed, 3 = run failed
    desc "unit", "run unit tests. NOTE: will not compile first, build to any target before running unit tests"
    def unit
      logfile =  File.join(ROOT_FOLDER, "Tmp", "test.unit.xml")

      # ensure the tmp folder exists
      FileUtils::mkdir 'Tmp' unless File.exists?('Tmp')

      # clean up from last time
      remove_file(logfile) if File.exists?(logfile)

      command = "#{unity_binary} -runEditorTests -batchMode -projectPath #{ROOT_FOLDER} -editorTestsResultFile #{logfile}"
      run(command)

      color = ($?.exitstatus == 0) ? :green : :red
      if File.exists?(logfile)
        # parse xml and show results
        File.open(logfile, "r") do |file|
          doc = Nokogiri::XML(file)
          doc.xpath('//test-results/@*').each do |element|
            #puts element.inspect
            say_status element.name, element.value, color
          end
        end
      else
        say_status "Run failed", "No log file created. See ~/Library/Logs/Unity/Editor.log", color
      end
    end

    private

    # where to start looking for templates, required by the template methods
    # even though we are using absolute paths
    def self.source_root
      File.dirname(__FILE__)
    end

  end
end

