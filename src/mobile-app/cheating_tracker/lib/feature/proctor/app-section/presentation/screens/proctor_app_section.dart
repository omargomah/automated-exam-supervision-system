import 'package:cheating_tracker/feature/proctor/alerts/presentation/screens/proctor_alerts_screen.dart';
import 'package:cheating_tracker/feature/proctor/dashboard/presentation/screens/proctor_dashboard_screen.dart';
import 'package:cheating_tracker/feature/proctor/profile/presentation/screens/proctor_profile_screen.dart';
import 'package:cheating_tracker/feature/proctor/exams/presentation/screens/proctor_exams_screen.dart';
import 'package:flutter/material.dart';

class ProctorAppSection extends StatefulWidget {
  const new({super.key});

  @override
  State<ProctorAppSection> createState() => _ProctorAppSectionState();
}

class _ProctorAppSectionState extends State<ProctorAppSection> {
  final List<Widget> _screens = const [
    ProctorDashboardScreen(),
    ProctorAlertsScreen(),
    ProctorExamsScreen(),
    ProctorProfileScreen(),
  ];
  final List<String> _titles = ['Dashboard', 'Alerts', 'Exams', 'Profile'];
  int _currIdx = 0;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(_titles[_currIdx])),
      body: _screens[_currIdx],

      bottomNavigationBar: BottomNavigationBar(
        backgroundColor: Colors.white,
        type: BottomNavigationBarType.fixed,
        currentIndex: _currIdx,
        onTap: (index) => setState(() {
          _currIdx = index;
        }),
        items: [
          BottomNavigationBarItem(
            icon: Icon(Icons.dashboard_outlined),
            label: "Dashboard",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.notifications_outlined),
            label: "Alerts",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.meeting_room_outlined),
            label: "Exams",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person_outlined),
            label: "Profile",
          ),
        ],
      ),
    );
  }
}
