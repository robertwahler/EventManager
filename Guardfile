require 'thor'
require File.expand_path("../tasks/basic_unity_helper", __FILE__)
require File.expand_path("../tasks/compile", __FILE__)
require File.expand_path("../tasks/test", __FILE__)

def notify(text, passed)
  image = passed ? :success : :failed
  Notifier.notify(text, title: 'Test Results', image: image)
end

# run limiter
last_run = Time.now
# minimum time between runs in msec
min_run_interval = 3000

run = proc do |_guard, _command, files|
  puts "Modified files: #{files[0]}" if files

  elapsed_time = ((Time.now - last_run).to_f * 1000.0).to_i
  if (elapsed_time > min_run_interval)
    args = []
    opts = {}

    # run the mcs compiler for syntax checking because it is fast, if this
    # fails, the next "heavy" Unity script will not run
    script = BasicUnity::Compile.new(args, opts)
    script.invoke("compile:test")

    # run the full on Unity unit test suite only if compile succeeds
    if ($?.exitstatus == 0)
      script = BasicUnity::Test.new(args, opts)
      script.invoke("test:unit")
      if ($?.exitstatus == 0)
        notify("Unit tests passed.", true)
      else
        notify("Unit tests failed. Check console.", false)
      end
    end
  end

  last_run = Time.now
end

yield_commands = {
  run_all: run,
  run_on_additions: run,
  run_on_modifications: run
}

## Only include directories we want to watch
directories %w(Assets).select{|d| Dir.exists?(d) ? d : UI.warning("Directory #{d} does not exist")}

guard :yield, yield_commands do
  watch(/(.*).cs/)
end
