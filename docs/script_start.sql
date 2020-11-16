USE [FargoSecurity]
GO
SET IDENTITY_INSERT [dbo].[Systems] ON 
GO
INSERT [dbo].[Systems] ([SystemId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedDate], [TimeSpan], [SystemType], [Code], [Name], [Path], [SecretToken], [PeriodTypeToken], [ExpirationTimeToken], [Status]) VALUES (1, N'TEMP', CAST(N'2020-11-07T18:55:14.4017903' AS DateTime2), NULL, NULL, N'1604793314', N'APP', N'S0', N'Fargo Security Web App Client', N'HTTP://www.fargosecurity.com', NULL, NULL, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[Systems] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RolId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedDate], [TimeSpan], [SystemId], [Name], [Status]) VALUES (1, N'TEMP', CAST(N'2020-11-07T19:01:53.7873456' AS DateTime2), NULL, NULL, N'1604793713', 1, N'Admin', 1)
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedDate], [TimeSpan], [UserType], [Name], [UserName], [Password], [Email], [Status]) VALUES (1, N'TEMP', CAST(N'2020-11-07T18:43:39.8699855' AS DateTime2), NULL, NULL, N'1604792619', N'INT', N'Administrator', N'admin', N'igx�pa���cv� ��', N'admin@fargoline.com', 1)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Profiles] ON 
GO
INSERT [dbo].[Profiles] ([ProfileId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedDate], [TimeSpan], [UserId], [RoleId], [NickName], [Status]) VALUES (1, N'TEMP', CAST(N'2020-11-07T19:03:52.8251681' AS DateTime2), NULL, NULL, N'1604793832', 1, 1, N'Fargo Cecurity Admin', 1)
GO
SET IDENTITY_INSERT [dbo].[Profiles] OFF
GO
