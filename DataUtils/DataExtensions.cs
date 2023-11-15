namespace DataUtils;

public class BufferReference
{
    public int BufferNumber { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
}

public class BufferManager<T>
{
    public int BufferSize { get; }

    public BufferManager(int bufferSize)
    {
        BufferSize = bufferSize;
    }

    public List<Buffer<T>> Buffers { get; private set; } = new();
    
    private Buffer<T> GetBuffer()
    {
        var buffer = new Buffer<T>
        {
            Data = new T[BufferSize]
        };
        Buffers.Add(buffer);
        return buffer;
    }

    public IEnumerable<BufferReference> InputData(IEnumerable<T> data)
    {
        var source = data.ToArray();
        if (source.Length <= BufferSize)
        {
            return new []{ InputDataSlice(source)};
        }
        else
        {
            var references = new List<BufferReference>();
            var fullSlices = source.Length / BufferSize;
            for (int i = 0; i < fullSlices; i++)
            {
                var slice = new T[BufferSize];
                Array.Copy(source, i*BufferSize, slice, 0, BufferSize);
                references.Add(InputDataSlice(slice));
            }

            if (source.Length % BufferSize != 0)
            {
                var slice = new T[source.Length - fullSlices * BufferSize];
                Array.Copy(source, fullSlices * BufferSize, slice, 0, slice.Length);
                references.Add(InputDataSlice(slice));
            }

            return references;
        }
    }

    public BufferReference InputDataSlice(IEnumerable<T> data)
    {
        
        var reference = new BufferReference();
        var source = data.ToArray();
        if (Buffers.Any(b => b.SpaceLeft >= source.Length))
        {
            for (int bufferIndex = 0; bufferIndex < Buffers.Count; bufferIndex++)
            {
                if (Buffers[bufferIndex].SpaceLeft >= source.Length)
                {
                    reference.BufferNumber = bufferIndex;
                    reference.Start = Buffers[bufferIndex].LastUsedIndex;
                    reference.Length = source.Length;
                    CopyDataToBuffer(source, Buffers[bufferIndex]);
                    break;
                }
            }
        }
        else
        {
            var buffer = GetBuffer();
            
            CopyDataToBuffer(source, buffer);
            reference.BufferNumber = Buffers.Count-1;
            reference.Start = 0;
            reference.Length = source.Length;
        }

        return reference;
    }

    private static void CopyDataToBuffer(T[] source, Buffer<T> buffer)
    {
        Array.Copy(source, 0, buffer.Data, buffer.LastUsedIndex, source.Length);
        buffer.LastUsedIndex += source.Length;
    }
}

public class Buffer<T>
{
    public T[] Data { get; set; }
    public int LastUsedIndex { get; set; }
    public int SpaceLeft => Data.Length - LastUsedIndex;
}
