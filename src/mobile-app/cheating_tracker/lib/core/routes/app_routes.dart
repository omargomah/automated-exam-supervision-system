import 'package:cheating_tracker/feature/proctor/app-section/presentation/screens/proctor_app_section.dart';
import 'package:flutter/material.dart';

abstract class AppRoutes {
  static const String initialRoute = 'proctor_app_section';

  static Map<String, Widget Function(BuildContext)> router() {
    return {'proctor_app_section': (context) => const ProctorAppSection()};
  }
}
