# WPF Theme Example
Example project for a WPF runtime theme implementation.

## Description
WPF allows for easily applying and changing the visual theme of an application through the use of resource dictionaries and the DynamicResource markup extension. This project aims to provide an example implementation of an approach to adding theme support.

## Build Requirements
- Visual Studio 2017

## Guide
First create a new WPF application or open an existing one.  Right-click on the project file of your application and select 
**Add->New Folder** and call it 'Themes'.  Right-click on the new folder and select **Add->Resource Dictionary** and call it 'DefaultStyle.xaml'.  Here is where we will add all of our base control styles.  Open up 'DefaultStyle.xaml' and let's start by adding a new style for TextBlocks and Buttons.
```xml
<!--General text-->
<Style x:Key="MyTextBlock" TargetType="TextBlock">
	   <Setter Property="FontStyle" Value="Normal"/>
	   <Setter Property="Foreground" Value="{DynamicResource GeneralTextColor}"/>
	   <Setter Property="FontFamily" Value="Gotham Rounded Book"/>
</Style>

<!--Button-->
<Style x:Key="MyButton" TargetType="{x:Type Button}">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="{x:Type Button}">
                <Grid x:Name="MainGrid">
                    <Border x:Name="OutsideBorder" BorderBrush="Black" BorderThickness="1"/>

                    <Grid Margin="4" VerticalAlignment="Center">
                        <ContentControl Content="{TemplateBinding Content}"/>
                    </Grid>
                </Grid>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```
> Note: This is by no means the only way to do this. You could define
> your style elements directly in App.xaml and forego using a separate
> resource dictionary but I like to separate it out since it makes it
> easier to swap out later if the situation comes up.

The key thing to notice here is the use of DynamicResource for the Foreground in MyTextBlock. A StaticResource will be evaluated once at load-time while the DynamicResource will be evaluated at run-time. This runtime evaluation is what allows us to change themes on the fly.

Next add another resource dictionary to the 'Themes' folder and call it 'DefaultTheme.xaml'. Open up the new file and add the following:
```xml
<SolidColorBrush x:Key="GeneralTextColor" Color="White"/>
<SolidColorBrush x:Key="BorderGeneralColor" Color="#FF201714"/>
```
Open up App.xaml and add our newly created resource dictionaries as entries in the merged dictionary.
```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Themes/DefaultStyle.xaml" />
            <ResourceDictionary Source="Themes/DefaultTheme.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```
Make note of where our theme is at in the merged dictionary.  We will use this later when switching themes. In our case this is the second element.

You could also define the default colors in the default style file, that way you don't have to define all the colors in subsequent themes. You have to define all the colors in this situation since we will be replacing the entire resource dictionary and if a particular resource doesn't exist in the new one it will not have a value and look transparent (Note: not the same thing as a transparent brush).

Now let's make another theme file. Right-click on the 'Themes' folder and add another resource dictionary and call it 'MyTheme.xaml'. Unlike the default theme, we need to make this into a resource. Do this by right-clicking on the MyTheme.xaml file and selecting 'Properties'. Change the 'Build Action' to 'Resource'.

Inside 'MyTheme.xaml' add:
```xml
<SolidColorBrush x:Key="GeneralTextColor" Color="DarkRed"/>
<SolidColorBrush x:Key="BorderGeneralColor" Color="DarkTurquoise"/>

<!--TextBlock-->
<Style x:Key="MyTextBlock" TargetType="TextBlock">
    <Setter Property="FontStyle" Value="Italic"/>
    <Setter Property="Foreground" Value="Beige"/>
</Style>

<!--Button-->
<Style x:Key="MyButton" TargetType="{x:Type Button}">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="{x:Type Button}">
                <Grid x:Name="MainGrid">
                    <Border x:Name="OutsideBorder"
                            BorderThickness="2"
                            CornerRadius="2">
                        <Border.BorderBrush>
                            <VisualBrush>
                                <VisualBrush.Visual>
                                    <Rectangle StrokeDashArray="4 2" Stroke="{DynamicResource BorderGeneralColor}"
                                               StrokeThickness="2"
                                               Width="{Binding ElementName=OutsideBorder, Path=ActualWidth}"
                                               Height="{Binding ElementName=OutsideBorder, Path=ActualHeight}"/>
                                </VisualBrush.Visual>
                            </VisualBrush>
                        </Border.BorderBrush>
                    </Border>

                    <Grid Margin="4" VerticalAlignment="Center">
                        <ContentControl Content="{TemplateBinding Content}"/>
                    </Grid>
                </Grid>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```
Now open up MainWindow.xaml and add a couple of buttons.
```XML
<Button Style="{StaticResource MyButton}">
    <TextBlock Text="Default"
               Foreground="{DynamicResource GeneralTextColor}"/>
</Button>

<Button Style="{DynamicResource MyButton}">
    <TextBlock Text="MyTheme"
               Foreground="{DynamicResource GeneralTextColor}"/>
</Button>
```
Finally, add a click handler to the buttons in  MainWindow.xaml:
```xml
<Button Style="{StaticResource MyButton}"
        Click="DefaultButton_Click">
    <TextBlock Text="Default"
               Foreground="{DynamicResource GeneralTextColor}"/>
</Button>
```
And in MainWindow.xaml.cs:
```c#
private void DefaultButton_Click(object sender, RoutedEventArgs e)
{
	// Theme is stored in the second entry of the merged dictionaries as listed in App.xaml
	Application.Current.Resources.MergedDictionaries[1].Source = new Uri(pack://application:,,,/WpfThemeExample;component/Themes/DefaultTheme.xaml, UriKind.RelativeOrAbsolute);
}
```
Repeat this for the second button, but in the code-behind, set it to MyTheme.xaml instead of the default theme.

![Default](/img/default_buttons.png)
![MyTheme](/img/mytheme_buttons.png)

As you can see here the button on the left retained its original style since it was set as a StaticResource. The button on the right however is updated to reflect the newly set style. The text color on both buttons has changed since that portion was set using a DynamicResource.

## Additional Notes
Instead of hard-coding theme locations into the application it can be made more dynamic by scanning them off of the hdd if you want to allow users to add their own themes for example.  You can also scan them from your application's resources or from another assembly's resources.
```c#
private void scanResources(string fileEnding = "Theme.xaml")
{
    var assembly = Assembly.GetExecutingAssembly();
    var resourceNames = assembly.GetManifestResourceNames();
    foreach (var resourceName in resourceNames)
    {
        ResourceSet set = new ResourceSet(assembly.GetManifestResourceStream(resourceName));
        foreach (DictionaryEntry item in set)
        {
            string fileName = item.Key.ToString();
            if (fileName.ToLower().EndsWith(fileEnding.ToLower()))
            {
                themes_.Add(new Theme() { Name = getNameFromPath(fileName), Path = "pack://application:,,,/WpfThemeExample;component/" + fileName });
            }
        }
    }
}
``` 

## Author
Edric Rominger edricr@gmail.com

## License
Unlicense license. See UNLICENSE file for details.