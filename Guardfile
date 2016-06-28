require 'thor'
require File.expand_path("../tasks/basic_unity_helper", __FILE__)
require File.expand_path("../tasks/compile", __FILE__)
require File.expand_path("../tasks/test", __FILE__)

# More info at https://github.com/guard/guard#readme

## Only include directories you want to watch
directories %w(Assets).select{|d| Dir.exists?(d) ? d : UI.warning("Directory #{d} does not exist")}

## Note: if you are using the `directories` clause above and you are not
## watching the project directory ('.'), then you will want to move
## the Guardfile to a watched dir and symlink it back, e.g.
#
#  $ mkdir config
#  $ mv Guardfile config/
#  $ ln -s config/Guardfile .
#
# and, you'll have to watch "config/Guardfile" instead of "Guardfile"

# Add files and commands to this file, like the example:
#   watch(%r{file/path}) { `command(s)` }
#
guard :shell do
  #watch(/(.*).txt/) {|m| `tail #{m[0]}` }
  watch(//) do |modified_files|
    puts "Modified files: #{modified_files.inspect}"

    # Run via system command
    # `thor test:nunit`

    # Run directly, faster and has color output
    args = []
    opts = {}
    script = BasicUnity::Test.new(args, opts)
    script.invoke("test:nunit")
  end
end
