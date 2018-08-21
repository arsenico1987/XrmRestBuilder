﻿using System.ComponentModel.Composition;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace XrmHackathon.XrmRestbuilder
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
    ExportMetadata("Name", "CRM Rest Builder for XrmToolBox (Preview)"),
    ExportMetadata("Description", "A port of the CRM Rest Builder for XrmToolbox "),
    // Please specify the base64 content of a 32x32 pixels image
    ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAITYAACE2AeEffZ4AAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAABrElEQVRYR+2Wv0sDMRTHr6KCiIhOIi6CtF7uWqGd1PsxWF1EcHF07aCL7SW12zk5OujgouCmuLg5Cf4DgovoppeoiLuIUNv4rjxwimAEp3zgceFDyL3LwTexDL9FSivzQN1pzsjKS1ToR63kMg67ed2d55GzdFUp9aDWJ2H5omDOB5TklBygViKYu8yp04Jqw/waan0SZs8JWDBtAOoctRLYqQrOlQl1dlDrI2EbBSUx1EkSZcdRK7ljuQFO7T34+kPeyA+hNhgMhm/SbE8id1FQe/ue5XKolYRh2O154ersrL8F4xHU+ogGmeDMee/EJSW3qJUEQeD7ftDy/VDC8wy1PklUGIe8fksbgEauUSuBt894XtDEBo5R6yMtK8M3iZdAvnOWG0X9A3EX/IJlaKRRKpUHURoMBsP/IapjfXAr3obkPOUbtoNaiaiSYZi/D3X0Ssnfzw643y9AZLc70U2dC9RKIGHXO+dMJ+rtPdT63FedKQFnR9oELLiLWslj3V2EK3kT5n9CE2uo9UnPjqfGZBbu+uWbmPSiViJjq+uZTRZ5jXjpGLUBsawvyjKwme/9YKAAAAAASUVORK5CYII="),
    ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAITYAACE2AeEffZ4AAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAADqElEQVR4Xu3cT0wcVRwH8EWjHmwUowdtqg2KyLzHgpGT2X9w8GBjQg9yI9o2lkOTRsK+t0u9bOpBTyr01noznoxH26aJPRuj7aFR01Jx982AlETtQVqNsbv9PfY3sMAz9DBDZur3k/xCducX3rwfb+ZNsssvAwAAAP9n88d7H/H1wEFfyUpDDUwsVIcf50OxCirZfYEWkzSuqpflSKuWeYAPpYeZyj5vlLjqa9kKw2ixUlcDr3JKLGjMQzTW353j+lpcWC4PPsopyffFeOZB+utf2jyJ9cmsxLUSF5V4mQr4r2tco+UZTks+U84OuyYRhqmItzk1UoGWp1zjrYWSf9lbCqcmW0PLN5yT4Agq4gSnRopW95eu8cKoK/E0pyZbvSI9umSarknYoAK+yamRolX2gWu8doib9tbCqYnXZZQ865oIFfbaDzXxMOdFqlHu66Ex/tw6pg06n5Oclg71d4e66a/+FZ34xkpU4jt6/QKnxCKoyhHaSIKN4ol/GkrMpvJRxgq096JR3pjdIWu7NAm7wo32CvZe3Jjqf4bfBgAAAAAAAAAAAAAAAIjR9Wn5bKDEp0ZJ42ux4Gs5tzTd9xQfjk2xWDyQz5cuFgrFG/Tz21yueITe7mofTYlfyt5+KtjyxuekHEosLM70P8lpkaOCHS0USi1HzHJKOtCq+3xb8cJQ8mNOi1SpVOqmVXfLUTwbzVxudIhTk63VynTRSvvDWTwKo8QVTo0UXbqvOQq3Hvl8cZpTk80W0Gj5u6t4NlDAe0BF+sxVPI6POC1S9hKmIq26ikfRzOfzg5yafEtV8Rytwu2biBY//xTjJkKFescWq6Nwa0GF/YRT0sN+6ZpWon2MuU73RPu95bk4d+AQXcqv02byNYWh+IZ25sP0droeYwAAAAAAAAAAAAAAAAASb62Dh/LGbC+Z3epb0LQtp6rZov2Xf/uJJL+dLmYm+4RR8lznZ9H0+jJFvE0ntBz1lVzqGPOOr8RsmlqeZFrttiebitcxofm4Oggtlwd7jBarznG1fJ/Tks9etq5JhEGTHOfUSNEl+6FrvHakqPHO4g6tn4z23uPUSFGR7o/WTw3d/4prAmEESr7FqZGi3z23dawwaNXfbsXUMSlyO7W/+63W+xinRopW2H+2v6PzOc1p6WAbMNKJ/7hlIr/uRgNGu9o6x6UN5HyqGjCGvp8cfsjXHrcA9SZWjok9fChWRr+017YApU1F2xZQ9qmADwEAAEDCZTJ3AU5ZNc2VM3mzAAAAAElFTkSuQmCC"),
    ExportMetadata("BackgroundColor", "White"),
    ExportMetadata("PrimaryFontColor", "Black"),
    ExportMetadata("SecondaryFontColor", "LightGray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }
    }
}