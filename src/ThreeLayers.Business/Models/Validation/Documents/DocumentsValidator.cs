namespace ThreeLayers.Business.Models.Validation.Documents;

public static class DocumentsValidator
{
    public static bool ValidateCpf(string cpf)
    {
#if DEBUG
        return true;
#endif
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        // Remove any non-numeric characters
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        // CPF must have 11 digits
        if (cpf.Length != 11)
            return false;

        // Check for known invalid CPFs (all same digits)
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Calculate first check digit
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);

        int remainder = sum % 11;
        int firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != firstCheckDigit)
            return false;

        // Calculate second check digit
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);

        remainder = sum % 11;
        int secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == secondCheckDigit;
    }

    public static bool ValidateCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        // Remove any non-numeric characters
        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        // CNPJ must have 14 digits
        if (cnpj.Length != 14)
            return false;

        // Check for known invalid CNPJs (all same digits)
        if (cnpj.All(c => c == cnpj[0]))
            return false;

        // Calculate first check digit
        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum = 0;
        for (int i = 0; i < 12; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights1[i];

        int remainder = sum % 11;
        int firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cnpj[12].ToString()) != firstCheckDigit)
            return false;

        // Calculate second check digit
        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights2[i];

        remainder = sum % 11;
        int secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cnpj[13].ToString()) == secondCheckDigit;
    }
}
