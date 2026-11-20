import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:flutter/material.dart';

class DashBoardWidget extends StatelessWidget {
  const new({
    super.key,
    required this.text,
    required this.icon,
    required this.iconColor,
    required this.count,
  });
  final String text;
  final IconData icon;
  final Color iconColor;
  final int count;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 36, vertical: 36),

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
      child: Center(
        child: Column(
          mainAxisAlignment: .spaceEvenly,
          children: [
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: iconColor.withAlpha(25),
              ),
              child: Icon(icon, color: iconColor, size: 30),
            ),
            Text(
              count.toString(),
              style: Theme.of(context).textTheme.bodyLarge!
                  .copyWith(fontSize: 30),
            ),
            Text(
              text,
              style: Theme.of(context).textTheme.bodyLarge!
                  .copyWith(color: Color(0xFF6A7282)),
            ),
          ],
        ),
      ),
    );
  }
}

