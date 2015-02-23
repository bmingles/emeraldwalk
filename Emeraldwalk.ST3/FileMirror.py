import sublime, sublime_plugin, subprocess

class FileMirror(sublime_plugin.EventListener):
	def __init__(self):
		self.config = sublime.load_settings(self.__class__.__name__+'.sublime-settings')

	def local_root(self):
		return self.config.get('local_root').lower()

	def remote_host(self):
		return self.config.get('remote_host')

	def remote_user(self):
		return self.config.get('remote_user')

	def remote_root(self):
		return self.config.get('remote_root').lower()

	def remote_path_separator(self):
		return self.config.get('remote_path_separator')

	def on_post_save(self, view):
		local_file = view.file_name().lower()
		local_root = self.local_root()
		remote_file = self.get_remote_file_path(local_file)

		print('local_file: ' + local_file)
		print('local_root: ' + local_root)
		print('remote_file: ' + remote_file)

		#self.run_process('plink', 'bingles@localhostx echo test');

	def get_remote_file_path(self, local_full_path):

		local_root = self.local_root()
		if not local_full_path.startswith(local_root):
			print ('Local path must be under local root')
			return

		relative_path = local_full_path[len(local_root):]
		remote_path = self.remote_root() + relative_path
		remote_path = remote_path.replace('\\', self.remote_path_separator())
		return remote_path

	def run_process(self, cmd, args):
		#print (self.config.get('local_root'))
		try:
			output = subprocess.check_output(cmd + ' ' + args, stderr=subprocess.STDOUT, shell=True)
		except subprocess.CalledProcessError as e:
			print('Exception: ', e.returncode, e.output)
		else:
			print (output)
