namespace Domain.Shared
{
    public static class Constants
    {
        public static class Classroom
        {
            public const int NameMaxLength = 50;
            public const int MinCapacity = 1;
        }

        public static class Seat
        {
            public const int MinLabel = 1;
            public const int MinCoordinate = 0;
            public const int TypeMaxLength = 50;
            public const int MinRow = 1;
            public const int MinSeatNumber = 1;
        }

        public static class CameraDevice
        {
            public const int MacAddressMaxLength = 17;
            public const int IpAddressMaxLength = 15;
            public const int StatusMaxLength = 50;
        }

        public static class Student
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int ImageUrlMaxLength = 2000;
            public const int ImagePublicIdMaxLength = 250;
        }

        public static class ExamSession
        {
            public const int TitleMaxLength = 100;
            public const int StatusMaxLength = 50;
        }

        public static class AlertEvent
        {
            public const int StatusMaxLength = 50;
            public const int AlertTypeMaxLength = 50;
            public const int ReviewerNoteMaxLength = 500;
            public const decimal MinConfidenceScore = 0.0m;
            public const decimal MaxConfidenceScore = 100.0m;
            public const int ConfidenceScorePrecision = 5;
            public const int ConfidenceScoreScale = 2;
        }

        public static class Evidence
        {
            public const int FrameTypeMaxLength = 50;
            public const int MediaPathMaxLength = 2000;
        }

        public static class ApplicationUser
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int EmailMaxLength = 150;
        }

        public static class AuditLog
        {
            public const int ActionMaxLength = 200;
            public const int EntityNameMaxLength = 50;
            public const int DetailsMaxLength = 500;
        }
    }
}
