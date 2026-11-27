import 'package:cached_network_image/cached_network_image.dart';
import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:flutter/material.dart';

class ProctorProfileScreen extends StatelessWidget {
  const new({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Container(
        margin: EdgeInsets.symmetric(horizontal: 16),
        padding: EdgeInsets.symmetric(horizontal: 16, vertical: 30),
        decoration: BoxDecoration(
          color: AppColors.containerColor,
          borderRadius: BorderRadius.circular(12),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withAlpha(10),
              blurRadius: 10,
              offset: const Offset(0, 4),
            ),
          ],
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,

          children: [
            ClipOval(
              child: CachedNetworkImage(
                fit: BoxFit.cover,
                height: 96,
                width: 96,
                imageUrl: "https://marketplace.canva.com/xHPJk/MAHA8XxHPJk/1/tl/canva-MAHA8XxHPJk.jpg",
                placeholder: (context, url) => SizedBox(
                  width: 24,
                  height: 24,
                  child: CircularProgressIndicator(
                    strokeWidth: 2,
                    color: AppColors.primaryColor,
                  ),
                ),
                errorWidget: (context, url, error) =>
                    Icon(Icons.error, color: AppColors.dangerColor),
              ),
            ),

            Text("Alex Mark", style: Theme.of(context).textTheme.bodyLarge),
            Text("proctor", style: Theme.of(context).textTheme.bodySmall),
            SizedBox(height: 36),
            GestureDetector(
              child: Container(
                padding: EdgeInsets.all(16),
                decoration: BoxDecoration(
                  color: AppColors.containerColor,
                  border: Border.all(width: 0.5, color: AppColors.dangerColor),
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Row(
                  mainAxisAlignment: .center,
                  children: [
                    Icon(Icons.logout, color: AppColors.dangerColor),
                    Text(
                      "Sign out",
                      style: Theme.of(context).textTheme.bodyLarge!
                          .copyWith(color: AppColors.dangerColor),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
