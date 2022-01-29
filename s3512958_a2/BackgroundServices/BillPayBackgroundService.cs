using System;
using s3512958_a2.Data;
using s3512958_a2.Models;
using Microsoft.EntityFrameworkCore;
namespace s3512958_a2.BackgroundServices
{
	public class BillPayBackgroundService:BackgroundService
	{
		private readonly IServiceProvider _services;
		private readonly ILogger<BillPayBackgroundService> _logger;

		public BillPayBackgroundService(IServiceProvider services, ILogger<BillPayBackgroundService> logger)
		{
			_services = services;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await DoWork(cancellationToken);

				_logger.LogInformation("Checking BillPay Rows... ... ...");

				await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
			}
		}

		private async Task DoWork(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Do Work... ...");
			using var scope = _services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MyContext>();

			var bills = await context.BillPay.Where(x => x.ScheduleTimeUtc <= DateTime.UtcNow).ToListAsync(cancellationToken);

            if (bills.Count>0)
            {
				foreach (var bill in bills)
				{
					var account = await context.Account.Where(x => x.AccountNumber == bill.AccountNumber).FirstAsync(cancellationToken);
					if (account.CalculateBalance() < bill.Amount)
					{
						_logger.LogInformation(bill.BillPayID +"Removing Billpay row(insuficient balance)");
						context.BillPay.Remove(bill);
					}
					else // Sufficient amount in acount
					{
						_logger.LogInformation(bill.BillPayID + "Paying bill");
						account.Balance -= bill.Amount;
						context.Update(account);
						context.Transaction.Add(
							new Transaction
							{
								TransactionType = 'B',
								AccountNumber = account.AccountNumber,
								Amount = bill.Amount,
								TransactionTimeUtc = DateTime.UtcNow
							});
						if (bill.Period.Equals('M'))
						{
							bill.ScheduleTimeUtc = bill.ScheduleTimeUtc.AddMonths(1);
							context.Update(bill);

						}
						else
						{
							context.BillPay.Remove(bill);
						}

					}
				}
			}
            
			await context.SaveChangesAsync(cancellationToken);


		}
	}
}

