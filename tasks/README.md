Thor
====

A working ruby and ruby gem install is required

Installation
------------

### requirements

If you are on Linux on OSX. No problem. Windows?  See trouble-shooting tips below.

    ruby 1.8 or 1.9

install

    gem install bundler
    bundle install

SSL errors on Windows
---------------------

    gem sources
    gem sources -a http://rubygems.org/

### ruby version

version should be 1.9.3-p392

### openssl version

version should be > 1.0.1

    ruby -ropenssl -e "puts OpenSSL::OPENSSL_VERSION"

### update to ruby gem 1.8.30

instructions

    https://gist.github.com/luislavena/f064211759ee0f806c88#installing-using-update-packages-new

remove all old versions, if any

    gem uninstall rubygems-update -x

manually download

    https://github.com/rubygems/rubygems/releases/download/v1.8.30/rubygems-update-1.8.30.gem

    gem install --local e:/downloads/rubygems-update-1.8.30.gem
    update_rubygems --no-ri --no-rdoc

After this, gem --version should report the new update version.

You can now salefy uninstall rubygems-update gem:

    gem uninstall rubygems-update -x

    Removing update_rubygems
    Successfully uninstalled rubygems-update-2.2.3

### install certs

where to install

    gem which rubygems

        c:/Users/robert/bin/ruby-1.9.3-p327-i386-mingw32/lib/ruby/site_ruby/1.9.1/rubygems.rb

manually download

NOTE: Difficult because wget doesn't work and chrome just shows the file, cut and paste to editor

    https://raw.githubusercontent.com/rubygems/rubygems/master/lib/rubygems/ssl_certs/AddTrustExternalCARoot-2048.pem

to this location

    c:/Users/robert/bin/ruby-1.9.3-p327-i386-mingw32/lib/ruby/site_ruby/1.9.1/rubygems/ssl_certs

or download to tmp and copy

    cp ~/tmp.pem c:/Users/robert/bin/ruby-1.9.3-p327-i386-mingw32/lib/ruby/site_ruby/1.9.1/rubygems/ssl_certs/AddTrustExternalCARoot-2048.pem

### still get SSL errors?


temporary edit

    Gemfile

change

    source "https://rubygems.org"

to
    source "http://rubygems.org"

now install via bundle

    bundle install

revert the unwanted changes

    git checkout Gemfile
    git checkout Gemfile.lock
    rm -rf .bundle/
