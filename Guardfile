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
#   watch(%r{file/path}) { `command(s)`

# run limiter
last_run = Time.now
# minimum time between runs in msec
min_run_interval = 3000

guard :shell do
  #watch(/(.*).txt/) {|m| `tail #{m[0]}` }
  watch(//) do |modified_files|
    puts "Modified files: #{modified_files.inspect}"

    elapsed_time = ((Time.now - last_run).to_f * 1000.0).to_i
    if (elapsed_time > min_run_interval)
      # Run via system command
      # `thor test:nunit`

      # Run directly, faster and has color output
      args = []
      opts = {}
      script = BasicUnity::Test.new(args, opts)
      script.invoke("test:nunit")
    end

    last_run = Time.now
  end
end
