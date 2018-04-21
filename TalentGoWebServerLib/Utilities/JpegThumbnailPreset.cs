using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace Changingsoft.Imaging
{
    /// <summary>
    /// 
    /// </summary>
	public class JpegThumbnailPreset : ThumbnailPreset
	{
        /// <summary>
        /// 
        /// </summary>
		public JpegThumbnailPreset()
			: base(ImageFormat.Jpeg, new System.Drawing.Size(300, 200), ResizeMode.Fit)
		{
			//Default Quility of JPEG Image.
			this.quility = 75L;
			this.EncoderParams.Add(new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, this.quility));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Quility"></param>
		public JpegThumbnailPreset(long Quility)
		{
			this.quility = Quility;
			this.EncoderParams.Add(new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, this.quility));
		}

		long quility;

        /// <summary>
        /// 
        /// </summary>
		public long Quility
		{
			get
			{
				return this.quility;
			}
			set
			{
				EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, value);
				this.EncoderParams[0] = parameter;
				this.quility = value;
			}
		}
	}
}
