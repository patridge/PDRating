# RatingView (PDRatingView)

RatingView lets you display an average rating and, optionally, collect a user's rating submission on items in your Xamarin.iOS application.

* Custom rating images.
* Custom rating scale.
* Transparent background for composing.

## Examples

You can use whatever images you want and whatever scale size you need. Many people use five stars.

![Five star rating scale](/component/Screenshots/five-stars-scale.png)

Others have something else entirely. Perhaps you want a 10-tomato rating.

![Ten tomato rating scale](/component/Screenshots/ten-tomatoes-scale.png)

Whatever you need, you give it a rectangle to fit into and it will resize things accordingly.

![Six moustaches rating scale](/component/Screenshots/six-moustaches-scale.png)

Ratings displays are kept minimal. If you need to compose your ratings view into something else, it will overlay it just fine.

![Star rating on a custom background](/component/Screenshots/custom-background.png)

## Usage

In any code-based `UIView` or `UIViewController`, you can add a `PDRatingView` to the displayed just like any other view.

    using PatridgeDev;
    ...
    
    PDRatingView ratingView;
    public override void ViewDidLoad() {
        
        // Gather up the images to be used.
        RatingConfig ratingConfig = new RatingConfig(
            emptyImage: UIImage.FromBundle("empty"),
            filledImage: UIImage.FromBundle("filled"),
            chosenImage: UIImage.FromBundle("chosen")
        );
        
        // Create the view.
        decimal averageRating = 3.25m;
        ratingView = new PDRatingView(new CGRect(0f, 0f, View.Bounds.Width, 125f), ratingConfig, averageRating);
        
        // [Optional] Do something when the user selects a rating.
        ratingView.RatingChosen += (sender, e) => {
            (new UIAlertView("Rated!", e.Rating.ToString() + " stars", null, "Ok")).Show();
        };
        
        // [Required] Add the view to the 
        View.Add(ratingView);
    }

## Other Configurations Options

### Between-item whitespace

Need some space between your rating items? Just set the `ItemPadding` in the `RatingConfig` object used to build the `PDRatingView`.

    // Put a little space between the rating items.
    ratingConfig.ItemPadding = 5f;

### Read-only (no user rating input)

If you are showing a rating without any intention of collecting a rating from the user, you can keep the rating view from taking any user input with the default iOS setting. As a result, this will keep it from ever triggering a `RatingChosen` event.

    // Only display the rating; don't allow user rating.
    ratingView.UserInteractionEnabled = false;
    
### Different rating scale size

Say you need users to rate things on a scale to ten. That can be changed in the `RatingConfig` object used to build the `PDRatingView`. The default is a 5-item scale of ratings.

    // Allow rating on a scale of 1 to 10.
    ratingConfig.ScaleSize = 10;
    
### Rounding of ratings to whole or half stars

If you want average ratings to display in half- or whole-star increments, that isn't currently built in to the `PDRatingView` system directly, but you can very easily use .NET to round appropriately before setting the view's `AverageRating` to reproduce the same result.

    decimal rating = 3.58m;
    decimal halfRoundedRating = Math.Round(rating * 2m, MidpointRounding.AwayFromZero) / 2m;
    decimal wholeRoundedRating = Math.Round(rating, MidpointRounding.AwayFromZero);
    StarRating.AverageRating = wholeRoundedRating;
