﻿using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Win32Interop.Enums;

namespace EasyTabs
{
	/// <summary>Renderer that produces tabs that mimic the appearance of the Chrome browser.</summary>
	public class ChromeTabRenderer : BaseTabRenderer
	{
        WindowsSizingBoxes _windowsSizingBoxes = null;

		/// <summary>Constructor that initializes the various resources that we use in rendering.</summary>
		/// <param name="parentWindow">Parent window that this renderer belongs to.</param>
		public ChromeTabRenderer(TitleBarTabs parentWindow)
			: base(parentWindow)
		{
			// Initialize the various images to use during rendering
			_activeLeftSideImage = Resources.ChromeLeft;
			_activeRightSideImage = Resources.ChromeRight;
			_activeCenterImage = Resources.ChromeCenter;
			_inactiveLeftSideImage = Resources.ChromeInactiveLeft;
			_inactiveRightSideImage = Resources.ChromeInactiveRight;
			_inactiveCenterImage = Resources.ChromeInactiveCenter;
			_closeButtonImage = Resources.ChromeClose;
			_closeButtonHoverImage = Resources.ChromeCloseHover;
			_background = Resources.ChromeBackground;
			_addButtonImage = new Bitmap(Resources.ChromeAdd);
			_addButtonHoverImage = new Bitmap(Resources.ChromeAddHover);

			// Set the various positioning properties
			CloseButtonMarginTop = 9;
			CloseButtonMarginLeft = 2;
            CloseButtonMarginRight = 4;
			AddButtonMarginTop = 3;
			AddButtonMarginLeft = -1;
			CaptionMarginTop = 9;
            IconMarginLeft = 9;
			IconMarginTop = 9;
			IconMarginRight = 5;
			AddButtonMarginRight = 45;

            _windowsSizingBoxes = new WindowsSizingBoxes(parentWindow);
        }

        public override int TabHeight
        {
            get
            {
                return _parentWindow.WindowState == FormWindowState.Maximized ? base.TabHeight : base.TabHeight + 8;
            }
        }

        /// <summary>Since Chrome tabs overlap, we set this property to the amount that they overlap by.</summary>
        public override int OverlapWidth
		{
			get
			{
				return 14;
			}
		}

        public override bool RendersSizingBox
        {
            get
            {
                return true;
            }
        }

        public override bool IsOverSizingBox(Point cursor)
        {
            return _windowsSizingBoxes.Contains(cursor);
        }

        public override HT NonClientHitTest(Message message, Point cursor)
        {
            HT result = _windowsSizingBoxes.NonClientHitTest(cursor);
            return result == HT.HTNOWHERE ? HT.HTCAPTION : result;
        }

        public override void Render(List<TitleBarTab> tabs, Graphics graphicsContext, Point offset, Point cursor, bool forceRedraw = false)
        {
            base.Render(tabs, graphicsContext, offset, cursor, forceRedraw);
            _windowsSizingBoxes.Render(graphicsContext, cursor);
        }

        protected override int GetMaxTabAreaWidth(List<TitleBarTab> tabs, Point offset)
        {
            return _parentWindow.ClientRectangle.Width - offset.X -
                        (ShowAddButton
                            ? _addButtonImage.Width + AddButtonMarginLeft + AddButtonMarginRight
                            : 0) -
                        (tabs.Count * OverlapWidth) -
                        _windowsSizingBoxes.Width;
        }
    }
}