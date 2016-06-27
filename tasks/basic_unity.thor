#!/usr/bin/env ruby

module BasicUnity

  def self.load_modules(dir)
    Dir.chdir(dir) do
      thor_files = Dir.glob('*.rb').delete_if { |x| not File.file?(x) }

      # load helpers first
      thor_files.each do |f|
        if f.match(/_helper\.rb$/)
          Thor::Util.load_thorfile(f)
        end
      end

      # load all the rest
      thor_files.each do |f|
        unless f.match(/_helper\.rb$/)
          Thor::Util.load_thorfile(f)
        end
      end

    end
  end

end

# load all Ruby files into the Thor::Sandbox
BasicUnity.load_modules(File.expand_path('..', __FILE__))
