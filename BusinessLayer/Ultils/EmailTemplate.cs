using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Ultils
{
	public static class EmailTemplate
	{
		public static string GenerateEmailTemplate(string userName, string link, string linkText)
		{
			return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <title>Password Reset Request</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 5px;
        }}
        .header {{
            background-color: #4caf50;
            color: #ffffff;
            padding: 20px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .content {{
            padding: 20px;
        }}
        .content p {{
            font-size: 16px;
            line-height: 1.5;
            color: #333333;
        }}
        .button {{
            display: inline-block;
            padding: 15px 25px;
            font-size: 16px;
            color: #ffffff;
            background-color: #4caf50;
            text-decoration: none;
            border-radius: 5px;
            text-align: center;
            margin-top: 20px;
        }}
        .footer {{
            text-align: center;
            padding: 10px;
            background-color: #f4f4f4;
            border-radius: 0 0 5px 5px;
        }}
        .footer p {{
            margin: 0;
            font-size: 14px;
            color: #555555;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Password Reset Request</h1>
        </div>
        <div class=""content"">
            <p>Hello {userName},</p>
            <p>We received a request to reset your password. Click the button below to set a new password:</p>
            <a href=""{link}"" class=""button"">{linkText}</a>
            <p>If you did not request a password reset, please ignore this email or contact support if you have any questions.</p>
        </div>
        <div class=""footer"">
            <p>&copy; {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
		}
	}
}
