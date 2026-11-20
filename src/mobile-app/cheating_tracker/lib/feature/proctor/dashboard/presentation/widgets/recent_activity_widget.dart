import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:flutter/material.dart';

class RecentActivityWidget extends StatelessWidget {
  const new({
    super.key,
    required this.text,
    required this.minutes,
    required this.color,
  });
  final String text;
  final int minutes;
  final Color color;
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Container(
        decoration: BoxDecoration(
          boxShadow: [
            BoxShadow(
              color: Colors.black.withAlpha(10),
              blurRadius: 10,
              offset: const Offset(0, 4),
            ),
          ],
          color: AppColors.containerColor,
          borderRadius: BorderRadius.circular(12),
        ),
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 16),
          child: Row(
            children: [
              Container(
                decoration: BoxDecoration(shape: BoxShape.circle, color: color),
                width: 8,
                height: 8,
              ),
              SizedBox(width: 12),
              Column(
                crossAxisAlignment: .start,
                children: [
                  Text(text, style: Theme.of(context).textTheme.bodyMedium),
                  Text(
                    "$minutes mins ago",
                    style: Theme.of(context).textTheme.bodySmall,
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
