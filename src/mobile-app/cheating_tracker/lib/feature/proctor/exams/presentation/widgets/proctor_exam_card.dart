import 'package:cheating_tracker/core/theme/app_colors.dart';
import 'package:flutter/material.dart';

class ProctorExamCard extends StatelessWidget {
  const new({
    super.key,
    required this.examName,
    required this.isActive,
    required this.studentsCount,
    required this.alertsCount,
  });

  final String examName;
  final bool isActive;
  final int studentsCount;
  final int alertsCount;
  @override
  Widget build(BuildContext context) {
    Offset? tapPosition;
    return GestureDetector(
      onTapDown: (details) {
        tapPosition = details.globalPosition;
      },
      onTap: () async {
        if (!isActive || tapPosition == null) return;

        final overlay =
            Overlay.of(context).context.findRenderObject() as RenderBox;

        final position = RelativeRect.fromLTRB(
          tapPosition!.dx,
          tapPosition!.dy,
          overlay.size.width - tapPosition!.dx,
          overlay.size.height - tapPosition!.dy,
        );

        final value = await showMenu<String>(
          context: context,
          position: position,
          items: [
            PopupMenuItem(value: 'Live', child: Text('Live')),
            PopupMenuItem(value: 'Students', child: Text('Students')),
          ],
        );
        if (value == null) return;
        switch (value) {
          case "Live":
            //print("Live")
            break;
          case "Students":
            //print(students)
            break;
        }
      },
      child: Container(
        margin: EdgeInsets.symmetric(vertical: 8),
        padding: EdgeInsets.all(16),
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
          children: [
            Row(
              mainAxisAlignment: .spaceBetween,
              crossAxisAlignment: .center,
              children: [
                Text(
                  examName,
                  style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                    color: isActive
                        ? AppColors.primaryColor
                        : AppColors.inActiveColor,
                  ),
                ),
                Container(
                  padding: EdgeInsets.all(8),
                  decoration: BoxDecoration(
                    color: isActive
                        ? AppColors.safeColor.withAlpha(12)
                        : AppColors.inActiveColor.withAlpha(12),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Text(
                    isActive ? "Active" : "Inactive",
                    style: TextStyle(
                      color: isActive
                          ? AppColors.safeColor
                          : AppColors.inActiveColor,
                    ),
                  ),
                ),
              ],
            ),
            Divider(color: AppColors.inActiveColor.withAlpha(20)),
            Row(
              children: [
                Icon(Icons.people_outlined, color: AppColors.inActiveColor),
                Text(
                  "$studentsCount students",
                  style: TextStyle(color: AppColors.inActiveColor),
                ),
                Spacer(),
                Icon(
                  Icons.notifications_outlined,
                  color: isActive
                      ? AppColors.dangerColor
                      : AppColors.inActiveColor,
                ),
                Text(
                  "$alertsCount Alerts",
                  style: TextStyle(
                    color: isActive
                        ? AppColors.dangerColor
                        : AppColors.inActiveColor,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
