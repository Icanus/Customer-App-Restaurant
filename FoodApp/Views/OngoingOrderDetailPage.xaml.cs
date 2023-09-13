using FoodApp.Helpers;
using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;
using Rectangle = Xamarin.Forms.Rectangle;

namespace FoodApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OngoingOrderDetailPage : ContentPage
    {
        private bool isAnimating = false;
        private Rectangle currentLayoutBounds;
        CustomMap customMap;
        //IService service => DependencyService.Get<IService>();
        OngoingOrderDetailViewModel viewModel;
        OrderParameter order;
        public OrderParameter Order 
        {
            get => order;
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Address { get; set; }
        public OngoingOrderDetailPage()
        {
            InitializeComponent();
            viewModel = new OngoingOrderDetailViewModel();
            BindingContext = viewModel;
            Communication();
            currentLayoutBounds = new Rectangle(0.5, 1, 1, 0.5);
            UpdateStackLayoutLayoutBounds();
            Device.BeginInvokeOnMainThread(() =>
            {
                tab1Button.BackgroundColor = Color.FromHex("29c8d6");
                tab2Button.BackgroundColor = Color.FromHex("d3d3d3");
                MapsDetails.IsVisible = false;
                AbsoluteStackLayout.IsVisible = false;
                OrderDetails.IsVisible = true;
                ImgMapImage.IsVisible = false;
                AbsoluteStackLayout.IsVisible = false;
                stackLayout.IsVisible = true;
            });
        }

        private void Tab1Button_Clicked(object sender, EventArgs e)
        {
            //tab1Content.IsVisible = true;
            //tab2Content.IsVisible = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                OrderDetails.IsVisible = true;
                MapsDetails.IsVisible = false;
                AbsoluteStackLayout.IsVisible = false;
                tab1Button.BackgroundColor = Color.FromHex("29c8d6");
                tab2Button.BackgroundColor = Color.FromHex("d3d3d3");
                ImgMapImage.IsVisible = false;
                stackLayout.IsVisible = true;
            });
        }

        private void Tab2Button_Clicked(object sender, EventArgs e)
        {
            //tab1Content.IsVisible = false;
            //tab2Content.IsVisible = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                OrderDetails.IsVisible = false;
                MapsDetails.IsVisible = true;
                AbsoluteStackLayout.IsVisible = true;
                tab2Button.BackgroundColor = Color.FromHex("29c8d6");
                tab1Button.BackgroundColor = Color.FromHex("d3d3d3");
                ImgMapImage.IsVisible = true;
                stackLayout.IsVisible = false;
            });
        }

        private async void OnSwipeUp(object sender, SwipedEventArgs e)
        {
            await AnimateLayoutChange(new Rectangle(0.5, 1, 1, 0.5));
        }

        private async void OnSwipeDown(object sender, SwipedEventArgs e)
        {
            await AnimateLayoutChange(new Rectangle(0.5, 1, 1, 0.03));
        }

        private async Task AnimateLayoutChange(Rectangle newLayoutBounds)
        {
            if (isAnimating)
                return;

            isAnimating = true;

            var animationDuration = 250; // Adjust the animation duration as needed
            var easing = Easing.CubicInOut; // Choose an easing function

            var startBounds = currentLayoutBounds; // Store the starting layout bounds
            var animation = new Animation(v =>
            {
                currentLayoutBounds = new Rectangle(
                    startBounds.X + (newLayoutBounds.X - startBounds.X) * v,
                    startBounds.Y + (newLayoutBounds.Y - startBounds.Y) * v,
                    startBounds.Width + (newLayoutBounds.Width - startBounds.Width) * v,
                    startBounds.Height + (newLayoutBounds.Height - startBounds.Height) * v);

                UpdateStackLayoutLayoutBounds();
            });

            animation.Commit(this, "LayoutAnimation", length: (uint)animationDuration, easing: easing);

            await Task.Delay(animationDuration);
            isAnimating = false;
        }

        private void UpdateStackLayoutLayoutBounds()
        {
            AbsoluteLayout.SetLayoutBounds(stackLayout, currentLayoutBounds);
        }

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "OngoingOrderLoadOrder");
            MessagingCenter.Subscribe<object, OrderParameter>(this, "OngoingOrderLoadOrder",async (args, id) =>
            {
                if (!viewModel.isFullyInitialize)
                    return;
                if (!string.IsNullOrEmpty(id.DriverId) && id.OrderId == Order.OrderId)
                {
                    viewModel.driverDetails = await JsonWebApiAction.GetVehicleByDriverId(Order.DriverId);
                    if (!string.IsNullOrEmpty(viewModel.Order.DriverId))
                    {
                        viewModel.IsDriverDetailAvailable = true;
                    }
                    else
                    {
                        viewModel.IsDriverDetailAvailable = false;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        viewModel.LoadOrder(id, true);
                    });
                    Order = id;
                    double lastlatitude, lastlongitude;
                    if (Order.IsChangeAddress)
                    {
                        lastlatitude = Convert.ToDouble(Order.ChangeAddressLat); //double.Parse("8.13463");
                        lastlongitude = Convert.ToDouble(Order.ChangeAddressLon); //double.Parse("-13.30254");
                        Address = Order.ChangeAddress;
                    }
                    else
                    {
                        lastlatitude = Convert.ToDouble(Order.Lat); //double.Parse("8.13463");
                        lastlongitude = Convert.ToDouble(Order.Lon); //double.Parse("-13.30254");
                    }
                    var thelatdecimal = lastlatitude.ToString().Split('.')[1];
                    var thelongdecimal = lastlongitude.ToString().Split('.')[1];
                    if (thelatdecimal.Length > 5)
                    {
                        thelatdecimal = thelatdecimal.Substring(0, 5);
                    }

                    if (thelongdecimal.Length > 5)
                    {
                        thelongdecimal = thelongdecimal.Substring(0, 5);
                    }
                    var thelat = lastlatitude.ToString().Split('.')[0] + "." + thelatdecimal;
                    var thelong = lastlongitude.ToString().Split('.')[0] + "." + thelongdecimal;

                    CustomPin devicePin = new CustomPin
                    {
                        Url = "",
                        Type = PinType.Place,
                        Position = new Position(lastlatitude, lastlongitude),
                        Label = $"customer",
                        Address = $"{Address}",
                        Name = $"customer"
                    };
                    CustomPin driverPin = new CustomPin
                    {
                        Name = $"{001}",
                        VehicleName = viewModel.driverDetails != null ? viewModel.driverDetails.CarDescription : "",
                        VehiclePhoto = viewModel.driverDetails != null ? viewModel.driverDetails.CarPhoto : "no_camera",
                        DriverName = $"{viewModel.driverDetails.FullName}",
                        DriverPhoto = viewModel.driverDetails != null ? viewModel.driverDetails.DriversPhoto : "no_camera",
                        Url = "",
                        Type = PinType.Place,
                        Position = new Position(Convert.ToDouble(Order.DriverLat), Convert.ToDouble(Order.DriverLon)),
                        Label = $"driver",
                        Address = $"Coordinate's: {Order.DriverLat}, {Order.DriverLon}"
                    };

                    if(customMap.Pins.Count > 0)
                    {
                        customMap.Pins.Clear();
                    }
                    List<CustomPin> customPinsList = new List<CustomPin>();
                    customPinsList.Add(devicePin);
                    customPinsList.Add(driverPin);

                    customMap.Pins.Add(devicePin);
                    customMap.Pins.Add(driverPin);

                    customMap.CustomPins = customPinsList;
                    try
                    {
                        if (string.IsNullOrEmpty(Order.DriverLat)) return;
                        var googleDirection = await JsonWebApiAction.GetMapsDirection(new Position(Convert.ToDouble(Order.DriverLat), Convert.ToDouble(Order.DriverLon)), new Position(lastlatitude, lastlongitude), null);
                        if (googleDirection != null)
                        {
                            try
                            {
                                var durations = googleDirection.routes.First().legs;
                                int totalDurationInMinutes = 0;
                                foreach (var leg in durations)
                                {
                                    var durationText = leg.duration.text.ToString();
                                    totalDurationInMinutes += StrHelper.ComputeDurationInMinutes(durationText);
                                }
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    viewModel.ETA = totalDurationInMinutes + " min(s)";
                                });
                            }
                            catch (Exception e)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    viewModel.ETA = "Calculating";
                                });
                            }
                            var polylinePositions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.routes.First().overview_polyline.points)));

                            var polyline = new Xamarin.Forms.Maps.Polyline
                            {
                                StrokeColor = Xamarin.Forms.Color.Blue,
                                StrokeWidth = 15
                            };
                            foreach (var p in polylinePositions.ToList())
                            {
                                polyline.Geopath.Add(p);
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                customMap.MapElements.Clear();
                                customMap.MapElements.Add(polyline);
                                viewModel.IsBusy = false;
                                viewModel.isFullyInitialize = true;
                            });
                        }
                    }
                    catch (InvalidOperationException exception)
                    {
                        viewModel.IsBusy = false;
                        IsBusy = false;
                    }
                    catch (Exception)
                    {
                        viewModel.IsBusy = false;
                        IsBusy = false;
                    }
                }
                if(id?.OrderId != null)
                {
                    if(id.OrderId == Order.OrderId)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            viewModel.LoadOrder(id, true);
                        });
                    }
                }
            });
        }

        private async void PrepareMap()
        {
            try
            {
                viewModel.IsBusy = true;
                await Task.Delay(300);
                viewModel.driverDetails = await JsonWebApiAction.GetVehicleByDriverId(Order.DriverId);
                if (!string.IsNullOrEmpty(viewModel.Order.DriverId))
                {
                    viewModel.IsDriverDetailAvailable = true;
                }
                else
                {
                    viewModel.IsDriverDetailAvailable = false;
                }
                double lastlatitude;
                double lastlongitude;
                lastlatitude = Convert.ToDouble(Lat); //double.Parse("8.13463");
                lastlongitude = Convert.ToDouble(Lon); //double.Parse("-13.30254");
                if (Order.IsChangeAddress)
                {
                    lastlatitude = Convert.ToDouble(Order.ChangeAddressLat); //double.Parse("8.13463");
                    lastlongitude = Convert.ToDouble(Order.ChangeAddressLon); //double.Parse("-13.30254");
                    Lat = lastlatitude.ToString();
                    Lon = lastlongitude.ToString();
                    Address = Order.ChangeAddress;
                }

                if (lastlatitude != 0)
                {
                    var position = new Position(lastlatitude, lastlongitude);
                    customMap = new CustomMap
                    {
                        MapType = MapType.Street,
                        IsShowingUser = false,
                        WidthRequest = 150,
                        HeightRequest = 150,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    customMap.MapType = MapType.Street;
                    customMap.TrafficEnabled = true;

                    if (mystacklayout.Children.Any())
                    {
                        mystacklayout.Children.Clear();
                    }


                    if (!string.IsNullOrEmpty(Order.DriverId))
                    {
                        var thelatdecimal = lastlatitude.ToString().Split('.')[1];
                        var thelongdecimal = lastlongitude.ToString().Split('.')[1];
                        if (thelatdecimal.Length > 5)
                        {
                            thelatdecimal = thelatdecimal.Substring(0, 5);
                        }

                        if (thelongdecimal.Length > 5)
                        {
                            thelongdecimal = thelongdecimal.Substring(0, 5);
                        }
                        var thelat = lastlatitude.ToString().Split('.')[0] + "." + thelatdecimal;
                        var thelong = lastlongitude.ToString().Split('.')[0] + "." + thelongdecimal;

                        CustomPin devicePin = new CustomPin
                        {
                            Url = "",
                            Type = PinType.Place,
                            Position = new Position(Convert.ToDouble(lastlatitude), Convert.ToDouble(lastlongitude)),
                            Label = $"customer",
                            Address = $"{Address}",
                            Name = $"customer"
                        };

                        CustomPin driverPin = new CustomPin
                        {
                            Name = $"{001}",
                            VehicleName = viewModel.driverDetails != null ? viewModel.driverDetails.CarDescription : "",
                            VehiclePhoto = viewModel.driverDetails != null ? viewModel.driverDetails.CarPhoto : "no_camera",
                            DriverName = $"{viewModel.driverDetails.FullName}",
                            DriverPhoto = viewModel.driverDetails != null ? viewModel.driverDetails.DriversPhoto : "no_camera",
                            Url = "",
                            Type = PinType.Place,
                            Position = new Position(Convert.ToDouble(Order.DriverLat), Convert.ToDouble(Order.DriverLon)),
                            Label = $"driver",
                            Address = $"Coordinate's: {Order.DriverLat}, {Order.DriverLon}"
                        };
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            customMap.Pins.Clear();
                        });
                        List<CustomPin> customPinsList = new List<CustomPin>();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            customMap.Pins.Add(driverPin);
                        });
                        customPinsList.Add(driverPin);

                        customPinsList.Add(devicePin);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            customMap.Pins.Add(devicePin);

                            customMap.CustomPins = customPinsList;
                        });

                        customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lastlatitude, lastlongitude), Xamarin.Forms.Maps.Distance.FromMiles(0.3)));
                        mystacklayout.Children.Add(customMap);
                        var googleDirection = await JsonWebApiAction.GetMapsDirection(new Position(Convert.ToDouble(Order.DriverLat), Convert.ToDouble(Order.DriverLon)), new Position(lastlatitude, lastlongitude), null);
                        if (googleDirection != null)
                        {
                            try
                            {
                                var durations = googleDirection.routes.First().legs;
                                int totalDurationInMinutes = 0;
                                foreach (var leg in durations)
                                {
                                    var durationText = leg.duration.text.ToString();
                                    totalDurationInMinutes += StrHelper.ComputeDurationInMinutes(durationText);
                                }
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    viewModel.ETA = totalDurationInMinutes + " min(s)";
                                });
                            }
                            catch (Exception e)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    viewModel.ETA = "Calculating";
                                });
                            }
                            var polylinePositions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.routes.First().overview_polyline.points)));

                            var polyline = new Xamarin.Forms.Maps.Polyline
                            {
                                StrokeColor = Xamarin.Forms.Color.Blue,
                                StrokeWidth = 15
                            };
                            foreach (var p in polylinePositions.ToList())
                            {
                                polyline.Geopath.Add(p);
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                customMap.MapElements.Clear();
                                customMap.MapElements.Add(polyline);
                            });
                        }
                    }
                    else
                    {
                        var thelatdecimal = lastlatitude.ToString().Split('.')[1];
                        var thelongdecimal =lastlongitude.ToString().Split('.')[1];

                        if (thelatdecimal.Length > 5)
                        {
                            thelatdecimal = thelatdecimal.Substring(0, 5);
                        }

                        if (thelongdecimal.Length > 5)
                        {
                            thelongdecimal = thelongdecimal.Substring(0, 5);
                        }

                        var thelat = lastlatitude.ToString().Split('.')[0] + "." + thelatdecimal;
                        var thelong = lastlongitude.ToString().Split('.')[0] + "." + thelongdecimal;

                        CustomPin devicePin = new CustomPin
                        {
                            Url = "",
                            Type = PinType.Place,
                            Position = new Position(Convert.ToDouble(Lat), Convert.ToDouble(Lon)),
                            Label = $"customer",
                            Address = $"{Address}",
                            Name = $"customer"
                        };

                        CustomPin driverPin = new CustomPin
                        {
                            Name = $"{001}",
                            VehicleName = viewModel.driverDetails != null ? viewModel.driverDetails.CarDescription : "",
                            VehiclePhoto = viewModel.driverDetails != null ? viewModel.driverDetails.CarPhoto : "no_camera",
                            DriverName = $"{viewModel.driverDetails.FullName}",
                            DriverPhoto = viewModel.driverDetails != null ? viewModel.driverDetails.DriversPhoto : "no_camera",
                            Url = "",
                            Type = PinType.Place,
                            Position = new Position(Convert.ToDouble(Globals.StoreLat), Convert.ToDouble(Globals.StoreLon)),
                            Label = $"driver",
                            Address = $"Coordinate's: {Globals.StoreLat}, {Globals.StoreLon}"
                        };
                        List<CustomPin> customPinsList = new List<CustomPin>();
                        customPinsList.Add(devicePin);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            customMap.CustomPins = customPinsList;
                            customMap.Pins.Add(devicePin);
                        });
                        customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lastlatitude, lastlongitude), Xamarin.Forms.Maps.Distance.FromMiles(0.3)));
                        mystacklayout.Children.Add(customMap);
                    }
                   
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    customMap.IsShowingUser = false;
                    viewModel.IsBusy = false;
                });
            }
            catch (InvalidOperationException exception)
            {
                viewModel.IsBusy = false;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                viewModel.IsBusy = false;
                IsBusy = false;
                // await DisplayAlert("Error", ex.Message, "OK").ConfigureAwait(true);
            }
            finally
            {
                viewModel.isFullyInitialize = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Order?.DateGmt != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.Order = Order;
                });
            }
            PrepareMap();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel == null) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                viewModel.MapImage = customMap.MapType != MapType.Street ? "satellite" : "street_view";
                customMap.MapType = customMap.MapType == MapType.Street ? MapType.Satellite : MapType.Street;
            });
        }
    }
}