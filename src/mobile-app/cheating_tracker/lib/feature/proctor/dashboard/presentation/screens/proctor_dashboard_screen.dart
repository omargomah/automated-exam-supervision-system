import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:cheating_tracker/feature/proctor/dashboard/presentation/widgets/dashboard_widget.dart';
import 'package:cheating_tracker/feature/proctor/dashboard/presentation/widgets/recent_activity_widget.dart';
import 'package:flutter/material.dart';

class ProctorDashboardScreen extends StatelessWidget {
  const new({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: .start,
        children: [
          Row(
            mainAxisAlignment: .spaceBetween,
            children: [
              DashBoardWidget(
                icon: Icons.warning_amber_rounded,
                count: 3,
                iconColor: AppColors.dangerColor,
                text: "Pending Alerts",
              ),
              DashBoardWidget(
                icon: Icons.remove_red_eye_outlined,
                count: 2,
                iconColor: AppColors.primaryColor,
                text: "Active Rooms",
              ),
            ],
          ),
          SizedBox(height: 16),
          Text("Recent Activity", style: Theme.of(context).textTheme.bodyLarge),
          SizedBox(height: 8),
          Expanded(
            child: ListView.builder(
              itemCount: 5,
              itemBuilder: (context, index) => RecentActivityWidget(
                text: "Alert cleared",
                minutes: index + 3,
                color: AppColors.safeColor,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
