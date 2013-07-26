using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PDRatingSample;

namespace PDRatingSample {
    public class CustomBackgroundViewController : UIViewController {
        PDRatingView ratingView;
        UIButton backgroundButton;
        string ratingStyle = "Background";

        public CustomBackgroundViewController() {
            Title = ratingStyle;
            TabBarItem.Image = UIImage.FromBundle("Stars/filled").Scale(new SizeF(30f, 30f));
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            View.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            View.BackgroundColor = UIColor.White;

            var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty"),
                                                filledImage: UIImage.FromBundle("Stars/filled"),
                                                chosenImage: UIImage.FromBundle("Stars/chosen"));
            // [Optional] Put a little space between the rating items.
            ratingConfig.ItemPadding = 5f;
            backgroundButton = UIButton.FromType(UIButtonType.RoundedRect);
            backgroundButton.SetBackgroundImage(UIImage.FromBundle("background/background").StretchableImage(0, 0), UIControlState.Normal);
            backgroundButton.Frame = new RectangleF(new PointF(24f, 24f), new SizeF(View.Bounds.Width - (2f * 24f), 125f));

            var ratingFrame = backgroundButton.Bounds;

            ratingView = new PDRatingView(ratingFrame, ratingConfig);

            // [Optional] Set the current rating to display.
            decimal rating = 3.58m;
            //decimal halfRoundedRating = Math.Round(rating * 2m, MidpointRounding.AwayFromZero) / 2m;
            //decimal wholeRoundedRating = Math.Round(rating, MidpointRounding.AwayFromZero);
            ratingView.AverageRating = rating;

            // [Optional] Make it read-only to keep the user from setting a rating.
            //StarRating.UserInteractionEnabled = false;

            // [Optional] Attach to the rating event to do something with the chosen value.
            ratingView.RatingChosen += (sender, e) => {
                (new UIAlertView("Rated!", e.Rating.ToString() + " stars", null, "Ok")).Show();
            };

            backgroundButton.Add(ratingView);
            View.Add(backgroundButton);
        }
    }
}