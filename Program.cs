using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandleAttribute
{
	class Program
	{
		static void Main(string[] args)
		{
			Demo demo = new Demo();
			try
			{
				demo.CallMe();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ExceptionHelper.GetDetailedMessage(ex));
			}
		}
	}

	class Demo
	{
		[MethodDescription("Call this to test the exception handling mechanism...")]
		public void CallMe()
		{
			this.TestException();
		}

		[MethodDescription("This is test exception method.")]
		private void TestException()
		{
			throw new Exception("Test Exception");
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	class MethodDescriptionAttribute : Attribute
	{
		string _description;
		public MethodDescriptionAttribute(string description)
		{
			this._description = description;
		}

		public string Description
		{
			get { return this._description; }
		}
	}

	class ExceptionHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string GetDetailedMessage(Exception ex)
		{
			StringBuilder str = new StringBuilder();
			List<MethodDescriptionAttribute> lst = GetStackTrace<MethodDescriptionAttribute>(ex);

			foreach (var item in lst)
			{
				str.Append(item.Description).AppendLine();
			}

			return str.Append(ex.Message).AppendLine().ToString();
		}

		/// <summary>
		/// Get the list of custom attributes.
		/// </summary>
		/// <typeparam name="T">CustomAttribute Type.</typeparam>
		/// <param name="ex">Exception object</param>
		/// <returns></returns>
		private static List<T> GetStackTrace<T>(Exception ex)
		{
			List<T> lst = new List<T>();
			StackTrace trace = new StackTrace(ex);

			if (trace != null && trace.FrameCount > 0)
			{
				for (int i = trace.FrameCount - 1; i >= 0; i--)
				{
					var attr = trace.GetFrame(i).GetMethod().GetCustomAttributes(typeof(T), true).FirstOrDefault();

					if (attr != null)
					{
						lst.Add((T)attr);
					}
				}
			}

			return lst;
		}
	}
}
