require "rake/clean"

CLEAN.include "*.xam"
CLEAN.include "xpkg"
CLEAN.include "PDRating.iOS.dll"
CLEAN.include "PDRating/bin"
CLEAN.include "PDRating/obj"
CLEAN.include "PDRatingSample/bin"
CLEAN.include "PDRatingSample/obj"

COMPONENT = "pdreview-1.0.0.xam"
MONOXBUILD = "/Library/Frameworks/Mono.framework/Commands/xbuild"

file "xpkg/xamarin-component.exe" do
	puts "* Downloading xamarin-component..."
	mkdir "xpkg"
	sh "curl -L https://components.xamarin.com/submit/xpkg > xpkg.zip"
	sh "unzip -o xpkg.zip -d xpkg"
	sh "rm xpkg.zip"
end

task :default => ["xpkg/xamarin-component.exe"] do
	line = <<-END
	mono xpkg/xamarin-component.exe package
	END
	puts "* Creating #{COMPONENT}..."
	puts line.strip.gsub "\t\t", "\\\n    "
	sh line, :verbose => false
	puts "* Created #{COMPONENT}"
end