using Checksum_files_test;

Console.WriteLine("Press 1 to WRITE a hash file of files within a directory\n" +
    "or\n" + "Press 2 to READ & VERIFY a hash file against the current files\n"+"or\n" + "Press 3 to show a list of all files in both root and and subdirectories");
ConsoleKeyInfo KeyedInfo = Console.ReadKey();
if (KeyedInfo.Key == ConsoleKey.D1)
{
    Console.Clear();
    //1. Enter directory, check it's not empty.
    //2. Get file list, display filenames + display side by side hash value
    //3. Write out the hash list as a hash.txt within this directory.

    Console.WriteLine("Enter Directory e.g. C:\\test to generate hash of each file");
    string SubmittedDirectoryPath = Console.ReadLine();
    while (SubmittedDirectoryPath == string.Empty)
    {
        Console.WriteLine("Please try again");
        SubmittedDirectoryPath = Console.ReadLine();
    }
    DirectoryInfo place = new DirectoryInfo(SubmittedDirectoryPath);
    FileInfo[] Files = place.GetFiles();
    Console.WriteLine("Readable Hash values for each file within " + SubmittedDirectoryPath);
    Console.WriteLine();
    Console.WriteLine("--------------------------");
    Console.WriteLine();

    using (StreamWriter outputFile = new StreamWriter(Path.Combine(SubmittedDirectoryPath, "Hash.txt")))
    {
        foreach (FileInfo i in Files)
        {
            Byte[] Result = CryptFunctions.ComputeMd5(SubmittedDirectoryPath + @"\" + i.Name);
            string output = BitConverter.ToString(Result).Replace("-", String.Empty).ToLower();


            Console.WriteLine(/*"File Name - {0}",*/ i.Name + " - " + output);
            outputFile.WriteLine(i.Name + " " + output);
        }
        Console.WriteLine();
        Console.WriteLine("Created Hash.txt in " + SubmittedDirectoryPath);
        //Console.ReadKey();

    }
}

if (KeyedInfo.Key == ConsoleKey.D2)
{
    Console.Clear();
    Console.WriteLine("Enter Directory e.g. C:\\test that has a hash file");
    string SubmittedDirectoryPath = Console.ReadLine();
    Console.WriteLine("Now enter the filename of the stored hashes to begin verifying");
    string HashFileName = Console.ReadLine();
    Console.WriteLine();
    Console.WriteLine("You entered " + SubmittedDirectoryPath + @"\" + HashFileName);
    Console.WriteLine();
    while (!File.Exists(SubmittedDirectoryPath + @"\" + HashFileName))
    {
        Console.WriteLine("File or directory not found");
        Console.WriteLine();
        Console.WriteLine("Re-open the application and start again");
        Console.WriteLine();
        Console.ReadKey();
    }
    int counteditems = 0;
    List<HashItems> HashFileContents = new();
    foreach (string line in File.ReadLines(SubmittedDirectoryPath + @"\" + HashFileName))
    {
        string[] splitline = line.Split(' ');
        HashItems HashItem = new()
        {
            FileName = splitline[0],
            Hash = splitline[1]
        };
        HashFileContents.Add(HashItem);
        counteditems++;
    }
    Console.WriteLine("Loaded " + counteditems + " hash lines");
    Console.WriteLine();
    foreach (HashItems item in HashFileContents)
    {
        Console.WriteLine(item.FileName + " - " + item.Hash);
    }
    Console.WriteLine("==================");
    Console.WriteLine();
    Console.WriteLine("Choose verification process");
    Console.WriteLine("Press 1 to verify files with the current directory contents of the same name");
    Console.WriteLine("or");
    Console.WriteLine("Press 2 to choose another directory to verify the files");
    ConsoleKeyInfo KeyedInfo2 = Console.ReadKey();
    if (KeyedInfo2.Key == ConsoleKey.D1)
    {
        Console.Clear();
        foreach (HashItems item in HashFileContents)
        {
            string[] file = Directory.GetFiles(SubmittedDirectoryPath, item.FileName);
            if (file.Length > 0)
            {
                //file is valid and matches
                Byte[] Result = CryptFunctions.ComputeMd5(SubmittedDirectoryPath + @"\" + item.FileName);
                string output = BitConverter.ToString(Result).Replace("-", String.Empty).ToLower();

                if (output == item.Hash)
                {
                    Console.WriteLine(item.FileName + " has a valid matching hash");

                }
                else
                {
                    Console.WriteLine(item.FileName + " has been altered and does not match the stored hash"); // Mark / flag this file as needing to be reverified (replaced and downloaded from the server)
                }
            }
            else
            {
                Console.WriteLine(
                item.FileName + " is missing or abnormal."); // Mark / flag this file as needing to be reverified (replaced and downloaded from the server
            }
        }
    }
    if (KeyedInfo2.Key == ConsoleKey.D2)
    {
        Console.Clear();
        Console.WriteLine("Not implemented yet, restart");

    }
   
}
if (KeyedInfo.Key == ConsoleKey.D3)
{
    Console.Clear();
    Console.WriteLine("Search the root folder and subfolders");
    Console.WriteLine("Enter path to search");
    string FolderandSub = Console.ReadLine();
    while (String.IsNullOrEmpty(FolderandSub))
    {
        Console.WriteLine("Enter path to search");
        FolderandSub = Console.ReadLine();
    }
    var DeepFilesFound = Directory.GetFiles(FolderandSub, "*.*", SearchOption.AllDirectories);
    //Console.WriteLine(DeepFilesFound);
    foreach(var DeepFile in DeepFilesFound)
    {
        int index = DeepFile.LastIndexOf(@"\");
        string DeepFile2 = DeepFile.Remove(0, index+1);
        Console.WriteLine(DeepFile2);
    }
}

