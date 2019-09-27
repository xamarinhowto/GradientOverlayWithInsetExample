# Introduction 

This solution contains a working example for a gradient overlay control with a start inset for your Xamarin apps.

# Outcome
![Gradient overlay with inset](gradientfeature.jpg "Gradient overlay with inset")


# Usage 
```xml
...
             xmlns:controls="clr-namespace:GradientOverlayWithInset.Controls"
...
        <controls:GradientOverlayView Grid.Row="0"
                                      HasGradientStartInset="true"
                                      GradientStartInsetPercent="0.20" />
...
```