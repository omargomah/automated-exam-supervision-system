import 'package:cheating_tracker/feature/proctor/exams/presentation/widgets/proctor_exam_card.dart';
import 'package:flutter/material.dart';

class ProctorExamsScreen extends StatelessWidget {
  const new({super.key});

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      padding: EdgeInsets.all(16),
      itemCount: 11,
      itemBuilder: (context, index) => ProctorExamCard(
        examName: "CS55101 Quiz",
        isActive: index % 2 == 0,
        alertsCount: index,
        studentsCount: 100 + index,
      ),
    );
  }
}
