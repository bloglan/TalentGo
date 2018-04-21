using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Changingsoft.Imaging
{
	/// <summary>
	/// 表示缩略图渲染预置配置。
	/// </summary>
	public class ThumbnailPreset
	{
		/// <summary>
		/// 默认构造函数。创建一个默认预置，该预置配有随机的Guid标识。
		/// </summary>
		public ThumbnailPreset()
			: this(ImageFormat.Jpeg, new Size(300, 200), ResizeMode.Fit)
		{
			
		}

		/// <summary>
		/// 使用指定的格式、缩略图尺寸和调整模式创建配置。
		/// </summary>
		/// <param name="Format"></param>
		/// <param name="ThumbnailSize"></param>
		/// <param name="Mode"></param>
		public ThumbnailPreset(ImageFormat Format, Size ThumbnailSize, ResizeMode Mode)
		{
			this.OutputFormat = Format;
			this.ThumbnailSize = ThumbnailSize;
			this.ThumbnailResizeMode = Mode;

			this.BackgroundColor = Color.Black;
			this.EncoderParams = new List<EncoderParameter>();
		}


		/// <summary>
		/// 获取指定格式的编码器支持的参数列表。
		/// </summary>
		/// <param name="format">要列出参数的图像的RawFormat。</param>
		/// <returns>返回编码器参数集合。如果找不到对应格式的编码器或编码器支持参数个数为0，返回一个空的编码器参数集合。</returns>
		public static EncoderParameters GetSupportedEncoderParameters(ImageFormat format)
		{
			EncoderParameters parameters = new EncoderParameters();
			using (Bitmap bmp = new Bitmap(1, 1))
			{
				try
				{
					return bmp.GetEncoderParameterList(GetCodec(format).Clsid);
				}
				catch
				{
					return new EncoderParameters();
				}
			}
		}

		/// <summary>
		/// 获取指定格式对应的编码器。
		/// </summary>
		/// <param name="format">图像的RawFormat。</param>
		/// <returns>如果找到编码器，则返回。否则返回null。</returns>
		public static ImageCodecInfo GetCodec(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}

        //EncoderParameters encoderParameters;

        /// <summary>
        /// 获取或设置缩略图的尺寸。
        /// </summary>
        public Size ThumbnailSize { get; set; }

        /// <summary>
        /// 获取或设置缩略图的输出格式。
        /// </summary>
        public ImageFormat OutputFormat { get; set; }

        /// <summary>
        /// 获取或设置缩略图的尺寸模式。
        /// </summary>
        public ResizeMode ThumbnailResizeMode { get; set; }

        /// <summary>
        /// 获取或设置缩略图背景色。
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// 获取编码器参数集合。
        /// </summary>
        public List<EncoderParameter> EncoderParams { get; }
    }
	
}
