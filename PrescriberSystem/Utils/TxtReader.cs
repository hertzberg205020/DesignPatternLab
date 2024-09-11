namespace PrescriberSystem.Utils;

public static class TxtReader
{
    public static List<string> ReadTxtFile(string filePath)
    {
        var lines = new List<string>();

        try
        {
            // 使用 File.ReadAllLines 方法讀取所有行
            // 這個方法會自動處理不同操作系統的換行符
            var fileLines = File.ReadAllLines(filePath);

            // 將所有行添加到列表中
            lines.AddRange(fileLines);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"錯誤: 文件 '{filePath}' 未找到。");
        }
        catch (IOException e)
        {
            Console.WriteLine($"讀取文件時發生錯誤: {e.Message}");
        }

        return lines;
    }
}
