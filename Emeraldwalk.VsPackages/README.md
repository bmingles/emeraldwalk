# emeraldwalk - File Mirror Utilities

## Overview
This extension allows running parameterized commands whenever files are saved in VS. It is best suited for scenarios where local file copy changes need to cause something to happy on a remote server such as pushing changes via sftp.

## Configuration
Configuration can be found under Tools -> Options -> Emeraldwalk -> File Mirror
* {LOCAL_ROOT} - Local root path (should mirror {REMOTE_ROOT})
* {REMOTE_HOST} - Remote server
* {REMOTE_USER} - Remote user
* {REMOTE_ROOT} - Remote root path (should mirror {LOCAL_ROOT})
* Remote Path Separator - Path separator character for remote file paths

## Commands
Multiple paramemeterized commands can be configured to execute after a file save. Curly brace tokens can be used to inject values into the commands.
* {LOCAL_FILE} - File saved in VS
* {LOCAL_ROOT}
* {REMOTE_HOST}
* {REMOTE_USER}
* {REMOTE_ROOT}
* {REMOTE_FILE}

## Example Commands
### PuTTY SCP (pscp.exe)
For secure file copy of saved file to remote server.

```
pscp "c:\localroot\dir\saved-file.txt" user@somehost.com:"/home/user/dir/saved-file.txt"
pscp "{LOCAL_FILE}" {REMOTE_USER}@{REMOTE_HOST}:"{REMOTE_FILE}"
```
