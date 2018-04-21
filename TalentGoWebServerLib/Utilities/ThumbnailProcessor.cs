using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Changingsoft.Imaging
{
	/// <summary>
	/// 表示一个缩略图处理器。
	/// </summary>
	public sealed class ThumbnailProcessor
	{
		private ThumbnailProcessor() { }

		/// <summary>
		/// 从输入的Image对象，用指定的预置生成缩略图。
		/// </summary>
		/// <param name="Input">要生成缩略图的Image对象。</param>
		/// <param name="Preset">有关缩略图生成过程参数的预置。</param>
		/// <param name="OutputStream">生成的缩略图输出到的流。</param>
		/// <returns>返回一个用指定编码器保存的图像的流。</returns>
		public static void RenderThumbnail(Image Input, ThumbnailPreset Preset, Stream OutputStream)
		{
			Rectangle CopyRectangle;
			Size RanderContainerSize;
			Rectangle RenderRectangle;
			CalculateSize(Input.Size, Preset.ThumbnailSize, Preset.ThumbnailResizeMode, out CopyRectangle, out RanderContainerSize, out RenderRectangle);
			
			using (Bitmap outbmp = new Bitmap(RanderContainerSize.Width, RanderContainerSize.Height))
			{
				using (Graphics g = Graphics.FromImage(outbmp))
				{
					//渲染背景色
					g.DrawRectangle(new Pen(Preset.BackgroundColor), new Rectangle(new Point(0, 0), RanderContainerSize));

					//渲染缩略图
					g.DrawImage(Input, RenderRectangle, CopyRectangle, GraphicsUnit.Pixel);

					//还应根据预置渲染水印图案。
				}

				//创建输出流。
				ImageCodecInfo Encoder = GetEncoder(Preset.OutputFormat);
				if (Encoder == null)
					throw new InvalidOperationException("操作无效。无法找到对应格式的编码器。");

				EncoderParameters encodeParams = new EncoderParameters(Preset.EncoderParams.Count);
				for (int i = 0; i < Preset.EncoderParams.Count; i++)
				{
					encodeParams.Param[i] = Preset.EncoderParams[i];
				}

				outbmp.Save(OutputStream, Encoder, encodeParams);
			}

		}

		private static ImageCodecInfo GetEncoder(ImageFormat format)
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

        /// <summary>
        /// 从输入的流用指定的预置生成缩略图。
        /// </summary>
        /// <param name="Input">要生成缩略图的图像的流。</param>
        /// <param name="Preset">有关缩略图生成过程参数的预置。</param>
        /// <param name="OutputStream"></param>
        /// <returns>返回一个用指定编码器保存的图像的流。</returns>
        /// <exception cref="ArgumentException">流没有包含有效的图像。</exception>
        public static void RenderThumbnail(Stream Input, ThumbnailPreset Preset, Stream OutputStream)
		{
			Image img = Image.FromStream(Input, true, true);
			RenderThumbnail(img, Preset, OutputStream);
		}

		/// <summary>
		/// 执行计算图像尺寸和矩形。
		/// </summary>
		/// <param name="OrginalImageSize">原始图像的尺寸。</param>
		/// <param name="TargetImageSize">期望的目标图像尺寸。</param>
		/// <param name="Mode">尺寸模式。</param>
		/// <param name="ImageCopyRectangle">原始图像裁剪矩形。</param>
		/// <param name="TargetContainerSize">目标图像容器尺寸。</param>
		/// <param name="TargetRenderRectangle">目标图像的渲染矩形。</param>
		private static void CalculateSize(Size OrginalImageSize, Size TargetImageSize, ResizeMode Mode, out Rectangle ImageCopyRectangle, out Size TargetContainerSize, out Rectangle TargetRenderRectangle)
		{
			//计算尺寸和矩形。
			//尺寸模式。
			//原始图像尺寸。OrginalImageSize
			//期望的目标图像尺寸。TargetImageSize
			//原始图像截取矩形。ImageCopyRectangle
			//目标图像容器尺寸。TargetContainerSize
			//目标图像的渲染矩形。TargetRenderRectangle
			ImageCopyRectangle = Rectangle.Empty;
			TargetContainerSize = Size.Empty;
			TargetRenderRectangle = Rectangle.Empty;

			double OrginalAspectRatio = (double)OrginalImageSize.Width / (double)OrginalImageSize.Height;
			double TargetAspectRatio = (double)TargetImageSize.Width / (double)TargetImageSize.Height;
			switch (Mode)
			{
				case ResizeMode.Fill:
					ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
					TargetContainerSize = TargetImageSize;
					TargetRenderRectangle = new Rectangle(0, 0, TargetContainerSize.Width, TargetContainerSize.Height);
					break;
				case ResizeMode.Fit:
					if (OrginalAspectRatio > TargetAspectRatio)
					{
						//源比例较大，应用目标宽，计算目标高。
						ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
						TargetContainerSize = TargetImageSize;
						double fitHeight = (double)TargetImageSize.Width / OrginalAspectRatio;
						double fitTop = ((double)TargetImageSize.Height - fitHeight) / 2;
						TargetRenderRectangle = new Rectangle(0, (int)Math.Floor(fitTop), TargetImageSize.Width, (int)Math.Floor(fitHeight));
					}
					else
					{
						//源比例较小，应用目标高，计算目标宽。
						ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
						TargetContainerSize = TargetImageSize;
						double fitWidth = (double)TargetImageSize.Height * OrginalAspectRatio;
						double fitLeft = ((double)TargetImageSize.Width - fitWidth) / 2;
						TargetRenderRectangle = new Rectangle((int)Math.Floor(fitLeft), 0, (int)Math.Floor(fitWidth), TargetImageSize.Height);
					}
					break;
				case ResizeMode.FitCut:
					if (OrginalAspectRatio > TargetAspectRatio)
					{
						//源比例较大，应用目标宽，计算目标高。
						ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
						double fitcutHeight = (double)TargetImageSize.Width / OrginalAspectRatio;
						TargetContainerSize = new Size(TargetImageSize.Width, (int)Math.Floor(fitcutHeight));
						TargetRenderRectangle = new Rectangle(0, 0, TargetImageSize.Width, (int)Math.Floor(fitcutHeight));
					}
					else
					{
						//源比例较小，应用目标高，计算目标宽。
						ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
						double fitcutWidth = (double)TargetImageSize.Height * OrginalAspectRatio;
						TargetContainerSize = new Size((int)Math.Floor(fitcutWidth), TargetImageSize.Height);
						TargetRenderRectangle = new Rectangle(0, 0, (int)Math.Floor(fitcutWidth), TargetImageSize.Height);
					}
					break;
				case ResizeMode.ScaleX:
					//强制宽，计算高
					ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
					double scaleHeight = (double)TargetImageSize.Width / OrginalAspectRatio;
					TargetContainerSize = new Size(TargetImageSize.Width, (int)Math.Floor(scaleHeight));
					TargetRenderRectangle = new Rectangle(0, 0, TargetImageSize.Width, (int)Math.Floor(scaleHeight));
					break;
				case ResizeMode.ScaleY:
					//强制高，计算宽
					ImageCopyRectangle = new Rectangle(0, 0, OrginalImageSize.Width, OrginalImageSize.Height);
					double scaleWidth = (double)TargetImageSize.Height * OrginalAspectRatio;
					TargetContainerSize = new Size((int)Math.Floor(scaleWidth), TargetImageSize.Height);
					TargetRenderRectangle = new Rectangle(0, 0, (int)Math.Floor(scaleWidth), TargetImageSize.Height);
					break;
				case ResizeMode.StretchFit:
					if (OrginalAspectRatio > TargetAspectRatio)
					{
						//计算截断宽。原图的截断矩形。
						double cutWidth = (double)OrginalImageSize.Height * TargetAspectRatio;
						double cutLeft = ((double)OrginalImageSize.Width - cutWidth) / 2;
						ImageCopyRectangle = new Rectangle((int)Math.Floor(cutLeft), 0, (int)Math.Floor(cutWidth), OrginalImageSize.Height);
						TargetContainerSize = TargetImageSize;
						TargetRenderRectangle = new Rectangle(new Point(0, 0), TargetImageSize);
					}
					else
					{
						//计算截断高
						double cutHeight = (double)OrginalImageSize.Width / TargetAspectRatio;
						double cutTop = ((double)OrginalImageSize.Height - cutHeight) / 2;
						ImageCopyRectangle = new Rectangle(0, (int)Math.Floor(cutTop), OrginalImageSize.Width, (int)Math.Floor(cutHeight));
						TargetContainerSize = TargetImageSize;
						TargetRenderRectangle = new Rectangle(new Point(0, 0), TargetImageSize);
					}
					break;
			}
			
		}
	}
}
