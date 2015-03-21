using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl
{
    public static class PerlTokens
    {
        private static IList<string> _operators;
        public static IList<string> Operators
        {
            get
            {
                return _operators ?? (_operators = new List<string>
                {
                    "->", "=>", "+", "-", "*", "/"
                });
            }
        }

        private static Regex _numberTokenPattern;
        public static Regex NumberTokenPattern
        {
            get
            {
                return _numberTokenPattern ?? (_numberTokenPattern = new Regex(@"[0-9]+(\.[0-9]*)?"));
            }
        }
        
        private static IList<string> _keywords;
        public static IList<string> Keywords
        {
            get
            {
                return _keywords ?? (_keywords = new List<string>
                {
                    "CORE", "__DATA__", "__END__", "__FILE__", "__LINE__", "__PACKAGE__", 
	                "and", "cmp", "continue", "do", "else", "elsif",
	                "eq", "exp", "for", "foreach", "ge", "gt", "if", "le", "lock",
	                "lt", "m", "ne", "no", "or", "package",
	                "q", "qq", "qr", "qw", "qx", "s", "sub", "tr",
	                "unless", "until", "while", "xor", "y"
                });
            }
        }

        private static IList<string> _functions;
        public static IList<string> Functions
        {
            get
            {
                return _functions ?? (_functions = new List<string>
                {
                    "-A", "-B", "-C", "-M", "-O", "-R", "-S", "-T", "-W", "-X"
                   , "-b", "-c", "-d", "-e", "-f", "-g", "-k", "-l", "-o", "-p", "-r", "-s", "-t", "-u", "-w", "-x", "-z"
                   , "AUTOLOAD", "BEGIN", "CHECK", "DESTROY", "END", "INIT", "UNITCHECK"
                   , "abs", "accept", "alarm", "atan2", "bind", "binmode", "bless", "break"
                   , "caller", "chdir", "chmod", "chomp", "chop", "chown", "chr", "chroot"
                   , "close", "closedir", "connect", "cos", "crypt", "dbmclose", "dbmopen"
                   , "defined", "delete", "die", "dump", "each", "endgrent", "endhostent"
                   , "endnetent", "endprotoent", "endpwent", "endservent", "eof", "eval", "exec"
                   , "exists", "exit", "fcntl", "fileno", "flock", "fork", "format", "formline"
                   , "getc", "getgrent", "getgrgid", "getgrnam", "gethostbyaddr", "gethostbyname"
                   , "gethostent", "getlogin", "getnetbyaddr", "getnetbyname", "getnetent"
                   , "getpeername", "getpgrp", "getppid", "getpriority", "getprotobyname"
                   , "getprotobynumber", "getprotoent", "getpwent", "getpwnam", "getpwuid"
                   , "getservbyname", "getservbyport", "getservent", "getsockname", "getsockopt"
                   , "glob", "gmtime", "goto", "grep", "hex", "index", "int", "ioctl", "join"
                   , "keys", "kill", "last", "lc", "lcfirst", "length", "link", "listen"
                   , "local", "localtime", "log", "lstat", "map", "mkdir", "msgctl", "msgget"
                   , "msgrcv", "msgsnd", "my", "next", "not", "oct", "open", "opendir", "ord"
                   , "our", "pack", "pipe", "pop", "pos", "print", "printf", "prototype", "push"
                   , "quotemeta", "rand", "read", "readdir", "readline", "readlink", "readpipe"
                   , "recv", "redo", "ref", "rename", "require", "reset", "return", "reverse"
                   , "rewinddir", "rindex", "rmdir", "say", "scalar", "seek", "seekdir", "select"
                   , "semctl", "semget", "semop", "send", "setgrent", "sethostent", "setnetent"
                   , "setpgrp", "setpriority", "setprotoent", "setpwent", "setservent", "setsockopt"
                   , "shift", "shmctl", "shmget", "shmread", "shmwrite", "shutdown", "sin", "sleep"
                   , "socket", "socketpair", "sort", "splice", "split", "sprintf", "sqrt", "srand"
                   , "stat", "state", "study", "substr", "symlink", "syscall", "sysopen", "sysread"
                   , "sysseek", "system", "syswrite", "tell", "telldir", "tie", "tied", "time"
                   , "times", "truncate", "uc", "ucfirst", "umask", "undef", "unlink", "unpack"
                   , "unshift", "untie", "use", "utime", "values", "vec", "wait", "waitpid"
                   , "wantarray", "warn", "write"
                });
            }
        }
    }
}
