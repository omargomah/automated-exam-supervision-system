import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:cheating_tracker/feature/proctor/alerts/presentation/widgets/proctor_alert_card.dart';
import 'package:flutter/material.dart';

class ProctorAlertsScreen extends StatelessWidget {
  const new({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(16),
      child: ListView.builder(
        itemCount: 7,
        itemBuilder: (context, index) => ProctorAlertCard(
          name: "John Doe",
          room: index + 100,
          alert: "Looking awya from camera",
          icon: Icons.camera_alt_outlined,
          severity: index % 2 == 0 ? "Critical" : "Warning",
          color: index % 2 == 0
              ? AppColors.dangerColor
              : AppColors.warningColor,
          time: "10:00 Am",
        ),
      ),
    );
  }
}
