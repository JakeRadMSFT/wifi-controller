using starlink_controller;

try
{
    do
    {


        var startlinkController = new Controller();
        await startlinkController.UpdateAsync();

        await Task.Delay(1000);

    } while (true);
}
catch (Exception ex)
{
    Console.WriteLine("Exception: " + ex.Message);
}