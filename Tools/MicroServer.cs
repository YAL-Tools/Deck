using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public class MicroServer {
		public HttpListener Listener = null;
		public Func<HttpListenerRequest, string> Handler;
		public bool Running = false;
		public MicroServer(Func<HttpListenerRequest, string> func) {
			Handler = func;
		}
		async void RunOuter() {
			try {
				Running = true;
				while (Running) {
					await RunOne();
				}
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}
		async Task RunOne() {
			var ctx = await Listener.GetContextAsync();
			var req = ctx.Request;
			var rsp = ctx.Response;

			Console.WriteLine(req.Url.ToString());
			var reply = Handler(req);
			var replyBytes = Encoding.UTF8.GetBytes(reply);
			rsp.AddHeader("Access-Control-Allow-Origin", "*");
			rsp.ContentType = "text/plain";
			rsp.ContentEncoding = Encoding.UTF8;
			rsp.ContentLength64 = replyBytes.LongLength;

			await rsp.OutputStream.WriteAsync(replyBytes, 0, replyBytes.Length);
			rsp.Close();
		}
		public bool Start(int port) {
			try {
				Listener = new HttpListener();
				Listener.Prefixes.Add($"http://localhost:{port}/");
				Listener.Prefixes.Add($"http://127.0.0.1:{port}/");
				Listener.Start();
			} catch (Exception e) {
				MessageBox.Show("Failed to start a micro-HTTP server."
					+ "\nDeckLightbox will not be able to expand/collapse a column automatically."
					+ "\nThis message can be safely ignored if you don't care about that.");
				return false;
			}
			RunOuter();
			return true;
		}
		public void Stop() {
			try {
				Listener.Close();
			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}
	}
}
