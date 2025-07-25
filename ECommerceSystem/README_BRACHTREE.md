﻿ECommerceSystem.sln
├── ECommerceSystem (ASP.NET Core MVC - Frontend)
│   ├── Controllers
│   │   ├── HomeController.cs
│   │   ├── ProductsController.cs
│   │   ├── OrdersController.cs
│   │   ├── AccountController.cs
│   │   ├── AdminController.cs
│   ├── Models
│   │   ├── ProductViewModel.cs
│   │   ├── OrderViewModel.cs
│   │   ├── UserViewModel.cs
│   │   ├── GuestViewModel.cs
│   │   ├── AdminViewModel.cs
│   │   ├── NotificationViewModel.cs
│   │   ├── CartViewModel.cs
│   │   ├── SuggestionViewModel.cs
│   │   ├── StatisticViewModel.cs
│   ├── Views
│   │   ├── Shared
│   │   │   ├── _AdminLayout.cshtml
│   │   │   ├── _CustomerLayout.cshtml
│   │   │   ├── _SellerLayout.cshtml
│   │   │   ├── _AccountLayout.cshtml
│   │   │   ├── _UserInfoPartial.cshtml
│   │   │   ├── LoginPartial.cshtml
│   │   │   ├── _CartDetail.cshtml
│   │   │   ├── _Suggestions.cshtml
│   │   │   ├── _Notifications.cshtml
│   │   │   └── _RoleBadgePartial.cshtml
│   │   ├── Home
│   │   │   ├── Index.cshtml
│   │   ├── Products
│   │   │   ├── Index.cshtml
│   │   │   ├── Detail.cshtml
│   │   ├── Orders
│   │   │   ├── Create.cshtml
│   │   │   ├── History.cshtml
│   │   │   ├── Success.cshtml
│   │   ├── Admin
│   │   │   ├── Dashboard.cshtml
│   │   ├── Account
│   │   │   ├── Login.cshtml Mô tả: Trang đăng nhập với xác thực người dùng.
│   │   │   ├── Register.cshtml
│   │   │   ├── Profile.cshtml
│   │   │   ├── ChangePassword.cshtml
│   │   ├── _ViewStart.cshtml Mô tả: Chọn layout động dựa trên vai trò người dùng, mặc định là _CustomerLayout.cshtml nếu không đăng nhập.
│   │   ├── _ViewImport.cshtml
│   ├── wwwroot
│   │   ├── css
│   │   │   ├── site.css
│   │   │   ├── layout.css
│   │   │   ├── cart.css
│   │   │   ├── suggestions.css
│   │   │   ├── admin.css
│   │   ├── js
│   │   │   ├── signalr.min.js
│   │   │   ├── chart.js
│   │   │   ├── analytics.js
│   │   ├── lib
│   ├── appsettings.json
│   ├── Program.cs
├── ECommerceSystem.Api (ASP.NET Core Web API - Backend)
│   ├── Controllers
│   │   ├── ProductsController.cs
│   │   ├── OrdersController.cs
│   │   ├── MobileController.cs
│   │   ├── AdminController.cs
│   │   ├── UserController.cs
│   │   ├── GuestController.cs
│   │   ├── SuggestionsController.cs
│   │   ├── StatisticsController.cs
│   │   ├── UserPreferencesController.cs
│   │   ├── LogsController.cs
│   │   ├── PromotionsController.cs
│   │   ├── UserLocationController.cs
│   │   ├── QrCodeController.cs
│   │   ├── SequenceController.cs
│   │   ├── NotificationsController.cs
│   │   ├── UserSessionsController.cs
│   │   ├── GuestSessionsController.cs
│   │   ├── UserSuggestionsController.cs
│   │   ├── AuthController.cs
│   │   ├── CartController.cs
│   │   ├── AnalyticsController.cs
│   ├── Data
│   │   ├── AppDbContext.cs
│   │   ├── Mongo
│   │   │   ├── MongoDbContext.cs
│   │   ├── Repositories
│   │   │   ├── ProductRepository.cs
│   │   │   ├── OrderRepository.cs
│   ├── Hubs
│   │   ├── NotificationHub.cs
│   ├── Services
│   │   ├── DataSyncService.cs
│   │   ├── AnalyticsService.cs
│   ├── appsettings.json
│   ├── Program.cs
├── ECommerceSystem.Shared (Class Library)
│   ├── DTOs
│   │   ├── ProductDTO.cs
│   │   ├── OrderDTO.cs
│   │   ├── OrderItemDTO.cs
│   │   ├── CartItemDTO.cs
│   │   ├── StatisticDTO.cs
│   │   ├── SequenceDTO.cs
│   │   ├── UserDTO.cs
│   │   ├── GuestSessionDTO.cs
│   │   ├── SuggestionDTO.cs
│   ├── Entities
│   │   ├── User.cs
│   │   ├── Product.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── GuestSession.cs
│   │   ├── UserPreference.cs
│   │   ├── Log.cs
│   │   ├── UserLocation.cs
│   │   ├── Promotion.cs
│   ├── Constants
│   │   ├── OrderStatus.cs
│   │   ├── Role.cs
│   ├── Interfaces
│   │   ├── IProductRepository.cs
│   │   ├── IOrderRepository.cs
│   │   ├── IAnalyticsRepository.cs
│   ├── Utilities
│   │   ├── QrCodeGenerator.cs
│   │   ├── StringHelper.cs