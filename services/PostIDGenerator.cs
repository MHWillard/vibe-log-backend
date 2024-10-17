using System;

namespace vibe_backend.services
{
    public class PostIDGenerator
    {
        static long GenerateID()
        {
            Random random = new Random();
            // Generate the first digit (1-9) to ensure it's not zero
            int firstDigit = random.Next(1, 10);

            // Generate the remaining 9 digits (0-9)
            long remainingDigits = 0;
            for (int i = 0; i < 9; i++)
            {
                remainingDigits = remainingDigits * 10 + random.Next(0, 10);
            }

            // Combine them to form a 10-digit number
            long tenDigitNumber = firstDigit * 1000000000L + remainingDigits;

            return tenDigitNumber;
        }
    }
}
