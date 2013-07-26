using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PDRatingSample {
    public class SampleTableViewControllerController : UITableViewController {
        public SampleTableViewControllerController() : base (UITableViewStyle.Plain) {
            Title = "Table";
            TabBarItem.Image = UIImage.FromBundle("Stars/filled").Scale(new SizeF(30f, 30f));
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            
            TableView.Source = new SampleTableViewControllerSource(50);
        }
    }
    public class SampleTableViewControllerSource : UITableViewSource {
        public int RowCount;
        public SampleTableViewControllerSource(int rowCount) {
            RowCount = rowCount;
        }

        public override int NumberOfSections(UITableView tableView) {
            return 1;
        }

        public override int RowsInSection(UITableView tableview, int section) {
            return RowCount;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
            tableView.CellAt(indexPath).Selected = false;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
            var cell = tableView.DequeueReusableCell(SampleTableViewControllerCell.Key) as SampleTableViewControllerCell;
            if (cell == null) {
                cell = new SampleTableViewControllerCell();
            }

            cell.Update(indexPath.Row % 6);

            return cell;
        }
    }
    public class SampleTableViewControllerCell : UITableViewCell {
        public static readonly NSString Key = new NSString("RatingsCell");
        PDRatingView ratingView;

        public void Update(int rating) {
            decimal halfRoundedRating = Math.Round(rating * 2m, MidpointRounding.AwayFromZero) / 2m;
            ratingView.AverageRating = halfRoundedRating;
        }

        public SampleTableViewControllerCell() : base(UITableViewCellStyle.Value1, Key) {
            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            BackgroundColor = UIColor.White;

            var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty"),
                                                filledImage: UIImage.FromBundle("Stars/filled"),
                                                chosenImage: UIImage.FromBundle("Stars/chosen"));
            ratingView = new PDRatingView(new RectangleF(PointF.Empty, ContentView.Bounds.Size), ratingConfig);
            ratingView.UserInteractionEnabled = false;

            ContentView.Add(ratingView);
        }
    }
}