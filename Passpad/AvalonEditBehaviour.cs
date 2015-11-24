using System;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;

namespace Passpad
{
	public sealed class AvalonEditBehaviour : Behavior<TextEditor>
	{
		public static readonly DependencyProperty TextBindingProperty =
			DependencyProperty.Register("TextBinding", typeof(string), typeof(AvalonEditBehaviour),
			new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

		public string TextBinding
		{
			get { return (string)GetValue(TextBindingProperty); }
			set { SetValue(TextBindingProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			if (AssociatedObject != null)
				AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (AssociatedObject != null)
				AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
		}

		private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
		{
			var textEditor = sender as TextEditor;
			if (textEditor?.Document != null)
				TextBinding = textEditor.Document.Text;
		}

		private static void PropertyChangedCallback(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var behavior = (AvalonEditBehaviour)dependencyObject;
			var editor = behavior.AssociatedObject as TextEditor;
			if (editor?.Document != null)
			{
				var caretOffset = editor.CaretOffset;
				editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
				editor.CaretOffset = Math.Max(0, Math.Min(dependencyPropertyChangedEventArgs.NewValue.ToString().Length, caretOffset));
			}
		}
	}
}
