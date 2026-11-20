import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:flutter/material.dart';

class ProctorAlertWidget extends StatelessWidget {
  const new({
    super.key,
    required this.name,
    required this.room,
    required this.alert,
    required this.icon,
    required this.severity,
    required this.color,
    required this.time,
  });
  final String name;
  final int room;
  final String alert;
  final IconData icon;
  final String severity;
  final Color color;
  final String time;

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.symmetric(vertical: 8),
      padding: EdgeInsets.all(12),
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
        spacing: 8,
        crossAxisAlignment: .stretch,
        children: [
          Row(
            children: [
              Text(name, style: Theme.of(context).textTheme.bodyLarge),
              Spacer(),
              Container(
                padding: EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                decoration: BoxDecoration(
                  color: color.withAlpha(25),
                  borderRadius: BorderRadius.circular(30),
                  border: Border.all(width: 0.5, color: color),
                ),
                child: Text(severity, style: TextStyle(color: color)),
              ),
            ],
          ),
          Text("Room $room", style: Theme.of(context).textTheme.bodySmall),
          Container(
            padding: EdgeInsets.all(8),
            decoration: BoxDecoration(
              color: AppColors.backgroundColor,
              borderRadius: BorderRadius.circular(12),
            ),

            child: Row(
              spacing: 4,
              children: [
                Icon(icon),
                Text(alert, style: Theme.of(context).textTheme.bodyMedium!),
              ],
            ),
          ),
          Row(
            children: [
              Spacer(),
              Text(time, style: Theme.of(context).textTheme.bodySmall),
            ],
          ),
        ],
      ),
    );
  }
}
