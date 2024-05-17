namespace fastfood_payment.Domain.Contants;

public static class ErrorMessages
{
    public static Dictionary<string, string> ErrorMessageList => _errorMessages;

    private static readonly Dictionary<string, string> _errorMessages = new()
    {
        { "PBE001", "Request inválido ou fora de especificação, vide documentação" },
        { "PBE002", "Nenhum pedido encontrado ou não está aguardando pagamento." },
        { "PBE003", "Falha em geração de QRCode." },
        { "PBE004", "Pagamento não encontrado." },
        { "PBE005", "Pagamento já processado." },
        { "PBE006", "Pagamento não se encontra pendente." },
        { "PBE007", "Falha em atualização de status do pedido." },
        { "PBE008", "Action precisa estar preenchido." },
        { "PBE009", "Action inválido para recebimento." },
        { "PBE010", "Data não pode ser nulo." },
        { "PBE011", "Id de data precisa estar preenchido." },
        { "PIE999", "Internal server error" }
    };
}
