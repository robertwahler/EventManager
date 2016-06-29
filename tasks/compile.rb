module BasicUnity

  class Compile < Thor
    namespace :compile
    include Thor::Actions
    include BasicUnity::BasicUnityHelper

    # adds :quiet, :skip, :pretent, :force
    add_runtime_options!

    desc "test", "compile nunit tests"
    def test
      command = "#{mcs_binary} \
                -debug \
                -recurse:'Assets/EventManager/Source/*.cs' \
                -recurse:'Assets/EventManager/Test/Editor/*.cs' \
                -recurse:'Assets/Examples/Colors/Scripts/*.cs' \
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

