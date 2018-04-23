var mkdirp = require('mkdirp');
var path = require('path');
var ncp = require('ncp');

// Paths
var srcDir = path.join(__dirname, '..', 'Assets', 'EventManager', 'Source');
var targetDir = path.join(__dirname, '..', '..', '..', 'Assets', 'packages', 'EventManager');

// Create folder if missing
mkdirp(targetDir, function (err) {
  if (err) {
    console.error(err)
    process.exit(1);
  }

  // Copy files
  ncp(srcDir, targetDir, function (err) {
    if (err) {
      console.error(err);
      process.exit(1);
    }
  });
});