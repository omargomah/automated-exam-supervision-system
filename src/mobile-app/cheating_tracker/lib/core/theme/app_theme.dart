import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:flutter/material.dart';

abstract class AppTheme {
  static final ThemeData lightTheme = ThemeData.light().copyWith(
    scaffoldBackgroundColor: AppColors.backgroundColor,
    appBarTheme: AppBarTheme(
      centerTitle: true,
      backgroundColor: AppColors.primaryColor,
      foregroundColor: Colors.white,
    ),
    textTheme: ThemeData.light().textTheme.copyWith(
      titleLarge: TextStyle(
        color: Colors.white,
        fontWeight: .w700,
        fontSize: 18,
      ),
      bodyLarge: TextStyle(
        color: AppColors.bodyFontColor,
        fontWeight: .w700,
        fontSize: 16,
      ),
      bodyMedium: TextStyle(
        color: AppColors.bodyFontColor,
        fontWeight: .w400,
        fontSize: 14,
      ),
      bodySmall: TextStyle(
        color: AppColors.hintFontColor,
        fontWeight: .w400,
        fontSize: 12,
      ),
    ),
  );
}
