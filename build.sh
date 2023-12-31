cat runtimes.txt | while read p 
do
  dotnet publish -c Release -r $p --self-contained true -p:PublishReadyToRun=true
  find bin/Release/net6.0/$p/publish -printf "%P\n" -type f -o -type l -o -type d | tar -czf out/$p.tar.gz --no-recursion -C bin/Release/net6.0/$p/publish -T -
done