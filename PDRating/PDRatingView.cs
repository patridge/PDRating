using System;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.CoreGraphics;
using System.Drawing;
using System.Collections.Generic;

namespace PDRatingSample {
    public class RatingConfig {
        public UIImage EmptyImage { get; set; }
        /// <summary>
        /// Image shown for the current average rating, when a rating is provided.
        /// </summary>
        public UIImage FilledImage { get; set; }
        /// <summary>
        /// Image shown when a user has chosen a rating.
        /// </summary>
        public UIImage ChosenImage { get; set; }
        /// <summary>
        /// Padding between the rendering of the items. The default is none (0f).
        /// </summary>
        public float ItemPadding { get; set; }
        /// <summary>
        /// Number of rating items in the scale (5 stars, 10 cows, whatever). The default is five.
        /// </summary>
        public int ScaleSize { get; set; }

        public RatingConfig(UIImage emptyImage, UIImage filledImage, UIImage chosenImage) {
            EmptyImage = emptyImage;
            FilledImage = filledImage;
            ChosenImage = chosenImage;
            ScaleSize = 5;
            ItemPadding = 0f;
        }
    }
    class RatingItemView : UIButton {
        UIImageView EmptyImageView;
        UIView FilledImageViewObscuringWrapper;
        UIView FilledImageViewSizingHolder;
        UIImageView FilledImageView;
        UIImageView SelectedImageView;
        private float _PercentFilled = 0f;
        public float PercentFilled {
            get {
                return _PercentFilled;
            }
            set {
                _PercentFilled = value;
                SetNeedsLayout();
            }
        }
        private bool _Chosen = false;
        public bool Chosen {
            get {
                return _Chosen;
            }
            set {
                _Chosen = value;
                SetNeedsLayout();
            }
        }
        public RatingItemView(UIImage emptyImage, UIImage filledImage, UIImage chosenImage) {
            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            EmptyImageView = new UIImageView(emptyImage) {
                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions,
            };
            Add(EmptyImageView);
            FilledImageView = new UIImageView(filledImage) {
                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions,
            };
            FilledImageViewSizingHolder = new UIView();
            FilledImageViewSizingHolder.Add(FilledImageView);
            FilledImageViewObscuringWrapper = new UIView() {
                ClipsToBounds = true,
                UserInteractionEnabled = false,
            };
            FilledImageViewObscuringWrapper.Add(FilledImageViewSizingHolder);
            Add(FilledImageViewObscuringWrapper);
            SelectedImageView = new UIImageView(chosenImage) {
                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions,
            };
            Add(SelectedImageView);
            PercentFilled = 0;
        }
        public override void LayoutSubviews() {
            base.LayoutSubviews();

            // Layout everything to their appropriate sizes.
            SelectedImageView.Frame = new RectangleF(PointF.Empty, Bounds.Size);
            EmptyImageView.Frame = new RectangleF(PointF.Empty, Bounds.Size);
            FilledImageViewObscuringWrapper.Frame = new RectangleF(PointF.Empty, Bounds.Size);
            FilledImageViewSizingHolder.Frame = new RectangleF(PointF.Empty, FilledImageViewObscuringWrapper.Bounds.Size);
            FilledImageView.Frame = new RectangleF(PointF.Empty, FilledImageViewSizingHolder.Bounds.Size);

            // Hide/Show things accordingly.
            if (Chosen) {
                // If selected, only show that view completely and hide the rest.
                SelectedImageView.SetAspectFitAsNeeded(UIViewContentMode.Center);
                EmptyImageView.Hidden = true;
                FilledImageViewObscuringWrapper.Hidden = true;
                SelectedImageView.Hidden = false;
            }
            else {
                // If not selected, hide selected, show empty and portion of filled.
                EmptyImageView.SetAspectFitAsNeeded(UIViewContentMode.Center);
                if (PercentFilled > 0f) {
                    FilledImageView.SetAspectFitAsNeeded(UIViewContentMode.Center);
                    if (PercentFilled < 1f) {
                        // Obscure a portion of the filled image based on the percent.
                        float revealWidth;
                        if (FilledImageView.Image.Size.Width < FilledImageView.Bounds.Width) {
                            revealWidth = ((FilledImageView.Bounds.Width - FilledImageView.Image.Size.Width) / 2f) + (FilledImageView.Image.Size.Width * PercentFilled);
                        }
                        else {
                            revealWidth = FilledImageView.Bounds.Width * PercentFilled;
                        }
                        FilledImageViewObscuringWrapper.Frame = new RectangleF(FilledImageViewSizingHolder.Frame.Location, new SizeF(revealWidth, FilledImageViewSizingHolder.Frame.Height));
                    }
                    FilledImageViewObscuringWrapper.Hidden = false;
                }
                else {
                    FilledImageViewObscuringWrapper.Hidden = true;
                }
                SelectedImageView.Hidden = true;
                EmptyImageView.Hidden = false;
            }
        }
    }
    public class RatingChosenEventArgs : EventArgs {
        public int Rating { get; set; }
        public RatingChosenEventArgs(int rating) {
            Rating = rating;
        }
    }
    public class PDRatingView : UIView {
        public event EventHandler<RatingChosenEventArgs> RatingChosen = delegate { };
        readonly RatingConfig StarRatingConfig;
        private decimal _AverageRating = 0m;
        public decimal AverageRating {
            get {
                return _AverageRating;
            }
            set {
                _AverageRating = value;
                SetNeedsLayout();
            }
        }
        private int? _ChosenRating = null;
        public int? ChosenRating {
            get {
                return _ChosenRating;
            }
            set {
                _ChosenRating = value;
                SetNeedsLayout();
            }
        }
        List<RatingItemView> StarViews;
        public PDRatingView(RectangleF frame, RatingConfig config) : this(frame, config, 0m) { }
        public PDRatingView(RectangleF frame, RatingConfig config, decimal averageRating) : this(config, averageRating) {
            Frame = frame;
        }
        public PDRatingView(RatingConfig config, decimal averageRating) : this(config) {
            AverageRating = averageRating;
        }
        public PDRatingView(RatingConfig config) {
            StarRatingConfig = config;
            StarViews = new List<RatingItemView>();
            ButtonsAndHandlers = new Dictionary<UIButton, EventHandler>();
            Enumerable.Range(0, StarRatingConfig.ScaleSize).ToList().ForEach(i => {
                int starRating = i + 1;
                RatingItemView starView = new RatingItemView(emptyImage: StarRatingConfig.EmptyImage,
                                                 filledImage: StarRatingConfig.FilledImage,
                                                 chosenImage: StarRatingConfig.ChosenImage);
                StarViews.Add(starView);
                EventHandler handler = (s, e) => {
                    ChosenRating = starRating;
                    RatingChosen(this, new RatingChosenEventArgs(ChosenRating.Value));
                };
                ButtonsAndHandlers.Add(starView, handler);
                Add(starView);
            });
            AssignHandlers();
        }
        public override void LayoutSubviews() {
            float starAreaWidth = Bounds.Width / StarRatingConfig.ScaleSize;
            float starAreaHeight = Bounds.Height - (2 * StarRatingConfig.ItemPadding);
            float starImageMaxWidth = starAreaWidth - (2 * StarRatingConfig.ItemPadding);
            float starImageMaxHeight = starAreaHeight - (2 * StarRatingConfig.ItemPadding);
            SizeF starAreaScaled = StarRatingConfig.EmptyImage.Size.ScaleProportional(starImageMaxWidth, starImageMaxHeight);
            float top = (Bounds.Height / 2f) - (starAreaScaled.Height / 2f);
            int i = 0;
            StarViews.ForEach(v => {
                float x = (i * starAreaWidth) + StarRatingConfig.ItemPadding;
                v.Frame = new RectangleF(new PointF(x, top), starAreaScaled);

                // Choose between showing a chosen rating and the average rating.
                if (ChosenRating != null) {
                    v.Chosen = ChosenRating.Value > i;
                    v.PercentFilled = 0f;
                }
                else {
                    v.Chosen = false;
                    float percentFilled = (AverageRating - 1) > i ? 1.0f : (float)(AverageRating - i);
                    v.PercentFilled = percentFilled;
                }
                i += 1;
            });

            base.LayoutSubviews();
        }
        Dictionary<UIButton, EventHandler> ButtonsAndHandlers { get; set; }
        protected void AssignHandlers() {
            ButtonsAndHandlers.ToList().ForEach(kvp => {
                UIButton button = kvp.Key;
                EventHandler handler = kvp.Value;
                button.TouchUpInside += handler;
            });
        }
        protected void RemoveHandlers() {
            ButtonsAndHandlers.ToList().ForEach(kvp => {
                UIButton button = kvp.Key;
                if (button != null) {
                    EventHandler handler = kvp.Value;
                    button.TouchUpInside -= handler;
                }
            });
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                RemoveHandlers();
            }
            base.Dispose(disposing);
        }
    }
}