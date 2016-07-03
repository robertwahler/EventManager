require 'nokogiri'
require 'fileutils'

module BasicUnity

  class Test < Thor
    namespace :test
    include Thor::Actions
    include BasicUnity::BasicUnityHelper

    # adds :quiet, :skip, :pretent, :force
    add_runtime_options!

    desc "guard", "watch source and run tests when either source or tests change"
    def guard
      run("bundle exec guard")
    end

    # Run unit tests
    # https://docs.unity3d.com/Manual/testing-editortestsrunner.html
    # 0 = succeeded, 2 = succeeded, some tests failed, 3 = run failed
    desc "unit", "run unit tests via Unity"
    method_option :verbose, :type => :boolean, :desc => "Show vebose information including all tests, even passing ones."
    def unit
      # ensure the tmp folder exists
      FileUtils::mkdir 'tmp' unless File.exists?('tmp')

      xml =  File.join(ROOT_FOLDER, "tmp", "EventManager.Tests.xml")
      logfile =  File.join(ROOT_FOLDER, "tmp", "Unity.Editor.log")
      target =  File.join(ROOT_FOLDER)
      lockfile = File.join(ROOT_FOLDER, "Temp", "UnityLockFile")

      if File.exists?(lockfile)
        # symlink Assets, Library, ProjectSettings, POSIX only
        say_status "shadow copy", "Lock file detected"
        target =  File.join(ROOT_FOLDER, "tmp", "shadow")
        shadow_copy(target)
      end

      # clean up from last time
      remove_file(xml) if File.exists?(xml)

      in_path(target) do
        #run("pwd")
        command = "#{unity_binary} -runEditorTests -nographics -batchMode -projectPath #{target} -editorTestsResultFile #{xml} -logFile #{logfile}"
        run(command)
      end

      color = ($?.exitstatus == 0) ? :green : :red
      if File.exists?(xml)
        # parse xml and show results
        File.open(xml, "r") do |file|
          doc = Nokogiri::XML(file)

          # results summary
          doc.xpath('//test-results/@*').each do |element|
            color = :green
            if (element.name == "failures")
              color = :red if (element.value != "0")
            end
            say_status element.name, element.value, color
          end

          puts

          # test names
          doc.css('test-case').each do |element|
            #puts element.inspect
            output = options[:verbose] ? true : false
            if (element.attr("success") == "True")
              color = :green
              msg = "#{element.attr("time")}"
            else
              color = :red
              output = true
              msg = "FAILED #{element.attr("time")}"
            end
            if output
              # strip off Test namespace
              name = element.attr("name").gsub(/.*\.Test\./, '')
              say_status name, msg, color
            end
          end

          puts

          # stack traces
          doc.css('stack-trace').each do |element|
            #trace = element.attr("name") #.gsub(/.*\.Test\./, '')
            #puts element.children.inspect
            say_status "Failure", element.children.text.chomp, :red
            #puts element.inspect
          end

        end
      else
        say_status "Run failed", "No xml file created. See #{logfile}", color
      end
    end

    desc "nunit", "run unit tests via NUnit-console, bypassing Unity (Non-Unity API calls only)"
    def nunit
      dll =  File.join(ROOT_FOLDER, "tmp", "EventManager.Tests.dll")
      xml =  File.join(ROOT_FOLDER, "tmp", "EventManager.Tests.xml")

      # remove the old files
      remove_file(dll) if File.exists?(dll)
      remove_file(xml) if File.exists?(xml)

      # build the dll
      invoke("compile:test")

      command = "export MONO_PATH=#{mono_path}; #{mono_binary} --debug #{nunit_binary} -nologo -noshadow #{dll} -xml=#{xml}"
      run_command(command)
    end

    private

    # Create a shadow copy of the project to get around one instance per project limit
    def shadow_copy(folder=nil)
      folder =  File.join(ROOT_FOLDER, "tmp", "shadow") unless folder
      FileUtils.mkdir_p(folder) unless File.exists?(folder)

      #
      # NOTE: symlinks don't work, need to copy the files
      #

      rsync_binary = '/usr/bin/rsync'
      if File.exists?(rsync_binary)
        from = File.join(ASSETS_FOLDER, '/')
        to = File.join(folder, "Assets")
        say_status "shadow copy", "#{from} #{to}"
        cmd = "rsync -a --delete #{from} #{to}"
        run(cmd)

        from = File.join(PROJECT_FOLDER, '/')
        to = File.join(folder, "ProjectSettings")
        say_status "shadow copy", "#{from} #{to}"
        cmd = "rsync -a --delete #{from} #{to}"
        run(cmd)
      else
        # TODO: Need a method to reset the destination i.e. rsync's '--delete'
        from = ASSETS_FOLDER
        to = File.join(folder, "Assets")
        say_status "shadow copy", "#{from} #{to}"
        FileUtils.copy_entry(from, to)

        from = PROJECT_FOLDER
        to = File.join(folder, "ProjectSettings")
        say_status "shadow copy", "#{from} #{to}"
        FileUtils.copy_entry(from, to)
      end
    end

    def unity_root
      "/Applications/Unity/Unity.app/Contents"
    end

    def mono_path
      paths = []
      paths << "#{unity_root}/Frameworks/Managed"
      paths << "#{unity_root}/UnityExtensions/Unity/EditorTestsRunner/Editor"
      paths << "#{unity_root}/UnityExtensions/Unity/GUISystem"
      paths.join(":")
    end

    def mono_binary
      "#{unity_root}/Frameworks/MonoBleedingEdge/bin/mono"
    end

    def nunit_binary
      "#{unity_root}/Frameworks/MonoBleedingEdge/lib/mono/unity/nunit-console.exe"
    end

    # where to start looking for templates, required by the template methods
    # even though we are using absolute paths
    def self.source_root
      File.dirname(__FILE__)
    end

  end
end

