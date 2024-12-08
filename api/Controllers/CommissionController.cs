using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommissionController : ControllerBase
    {
        private const decimal LOCAL_SALES_COMMISSION_RATE = 0.20m;
        private const decimal FOREIGN_SALES_COMMISSION_RATE = 0.35m;
        private const decimal COMPETITOR_LOCAL_SALES_COMMISSION_RATE = 0.02m;
        private const decimal COMPETITOR_FOREIGN_SALES_COMMISSION_RATE = 0.0755m;

        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [HttpPost]
        public IActionResult Calculate([FromBody] CommissionCalculationRequest calculationRequest)
        {
            if (calculationRequest == null ||
                calculationRequest.LocalSalesCount < 0 ||
                calculationRequest.ForeignSalesCount < 0 ||
                calculationRequest.AverageSaleAmount < 0)
            {
                return BadRequest("Invalid input parameters");
            }

            // Calculate FCamara Commissions
            decimal localSalesCommission = CalculateCommission(
                calculationRequest.LocalSalesCount,
                calculationRequest.AverageSaleAmount,
                LOCAL_SALES_COMMISSION_RATE
            );

            decimal foreignSalesCommission = CalculateCommission(
                calculationRequest.ForeignSalesCount,
                calculationRequest.AverageSaleAmount,
                FOREIGN_SALES_COMMISSION_RATE
            );

            decimal fCamaraCommissionAmount = localSalesCommission + foreignSalesCommission;

            // Calculate Competitor Commissions
            decimal competitorLocalSalesCommission = CalculateCommission(
                calculationRequest.LocalSalesCount,
                calculationRequest.AverageSaleAmount,
                COMPETITOR_LOCAL_SALES_COMMISSION_RATE
            );

            decimal competitorForeignSalesCommission = CalculateCommission(
                calculationRequest.ForeignSalesCount,
                calculationRequest.AverageSaleAmount,
                COMPETITOR_FOREIGN_SALES_COMMISSION_RATE
            );

            decimal competitorCommissionAmount = competitorLocalSalesCommission + competitorForeignSalesCommission;

            return Ok(new CommissionCalculationResponse
            {
                FCamaraCommissionAmount = Math.Round(fCamaraCommissionAmount, 2),
                CompetitorCommissionAmount = Math.Round(competitorCommissionAmount, 2)
            });
        }

        private static decimal CalculateCommission(int salesCount, decimal averageSaleAmount, decimal commissionRate)
        {
            return salesCount * averageSaleAmount * commissionRate;
        }
    }

    public class CommissionCalculationRequest
    {
        public int LocalSalesCount { get; set; }
        public int ForeignSalesCount { get; set; }
        public decimal AverageSaleAmount { get; set; }
    }

    public class CommissionCalculationResponse
    {
        public decimal FCamaraCommissionAmount { get; set; }
        public decimal CompetitorCommissionAmount { get; set; }
    }
}