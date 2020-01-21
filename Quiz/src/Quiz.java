import java.security.MessageDigest;
import java.math.BigInteger;
import java.util.Arrays;
import java.util.Objects;
import java.util.function.DoubleUnaryOperator;
import java.util.*;
import java.util.stream.IntStream;
import java.util.stream.Stream;
import java.util.function.Predicate;
import java.util.function.Function;
import java.util.function.BiConsumer;
import java.io.*;
import java.nio.charset.Charset;

public class Quiz {
    public static void main(String[] args) throws Exception {
        MessageDigest md = MessageDigest.getInstance("MD5");
        byte[] digest = md.digest("abracadabra".getBytes("UTF-8"));
        for (byte b : digest) {
            System.out.printf("%02x", b);
        }

        System.out.println(booleanExpression(true,false,false,false));
        System.out.println(0x0bp3);
        System.out.println(flipBit(13,1));

        System.out.println(isPowerOfTwo(1));

        System.out.println(Arrays.toString(mergeArrays(new int[]{1,2},new int[]{0,1,1})));

        InputStream stream;
        int result;
        stream = getStream( new byte[] { 0x33, 0x45, 0x01});

        result = checkSumOfStream(stream);
        System.out.print(result);
    }

    public static boolean booleanExpression(boolean a, boolean b, boolean c, boolean d) {
        int s=0;
        s+=a?1:0;
        s+=b?1:0;
        s+=c?1:0;
        s+=d?1:0;

        return s==2;
    }
    /**
     * Flips one bit of the given <code>value</code>.
     *
     * @param value     any number
     * @param bitIndex  index of the bit to flip, 1 <= bitIndex <= 32
     * @return new value with one bit flipped
     */
    public static int flipBit(int value, int bitIndex) {
        int v=(int)Math.pow(2,bitIndex-1);
        return value^v; // put your implementation here
    }

    /**
     * Checks if given <code>value</code> is a power of two.
     *
     * @param value any number
     * @return <code>true</code> when <code>value</code> is power of two, <code>false</code> otherwise
     */
    public static boolean isPowerOfTwo(int value) {
        value=Math.abs(value);
        if(value==0) return false;
        return (value&(value-1))==0;
    }

    /**
     * Checks if given <code>text</code> is a palindrome.
     *
     * @param text any string
     * @return <code>true</code> when <code>text</code> is a palindrome, <code>false</code> otherwise
     */
    public static boolean isPalindrome(String text) {

       var txt=text.replaceAll("[^A-Za-z1-9]+", "").toLowerCase().toCharArray();
for(int i=0;i<txt.length/2;i++)
    if(txt[i]!=txt[txt.length-1-i])
        return  false;

        return true; // your implementation here
    }

    /**
     * Calculates factorial of given <code>value</code>.
     *
     * @param value positive number
     * @return factorial of <code>value</code>
     */
    public static BigInteger factorial(int value) {
        if(value==1) return BigInteger.ONE;
        return  BigInteger.valueOf(value).multiply(factorial(value-1)); // your implementation here
    }


    /**
     * Merges two given sorted arrays into one
     *
     * @param a1 first sorted array
     * @param a2 second sorted array
     * @return new array containing all elements from a1 and a2, sorted
     */
    public static int[] mergeArrays(int[] a1, int[] a2) {
    if(a1.length==0) return  a2;
    if(a2.length==0) return a1;

        int[] res=new int[a1.length+a2.length];
        int i1=0,i2=0,i=0;
        for(;i<res.length;i++)
        {
            if(a1[i1]<a2[i2])
                res[i]=a1[i1++];
            else
                res[i]=a2[i2++];

            if(i1==a1.length||i2==a2.length)
                break;
        }
        i++;
        if(i1==a1.length){
            for(;i<res.length;i++)
                res[i]=a2[i2++];
        }
            else
                for(;i<res.length;i++)
            res[i]=a1[i1++];


        return res; // your implementation here
    }

    public enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public class Robot {
        int x=0;
        int y=0;
        Direction direction = Direction.UP;

        public Robot(int a,int b, Direction d){
            x=a;
            y=b;
            direction=d;
        }

        public Direction getDirection() {
            return direction;
        }

        public int getX() {
            return x;
        }

        public int getY() {
            return y;
        }

        public void turnRight() {
            switch (getDirection()){
                default:
                case DOWN: direction = Direction.LEFT;
                    break;
                case UP: direction = Direction.RIGHT;
                    break;
                case RIGHT: direction = Direction.DOWN;
                    break;
                case LEFT: direction = Direction.UP;
            }
        }
        public void stepForward() {
            switch (getDirection()){
                default:
                case RIGHT: x++;
                    break;
                case LEFT: x--;
                    break;
                case UP: y++;
                    break;
                case DOWN: y--;
                    break;
            }

    }
}
    public static void moveRobot(Robot robot, int toX, int toY) {

        if (robot.getY() < toY) {

            while (robot.getDirection() != Direction.UP) robot.turnRight();

            while (robot.getY() < toY) robot.stepForward();

        }
        else if  (robot.getY() > toY) {

            while (robot.getDirection() != Direction.DOWN) robot.turnRight();

            while (robot.getY() > toY) robot.stepForward();

        }

        if (robot.getX() < toX) {

            while (robot.getDirection() != Direction.RIGHT) robot.turnRight();

            while (robot.getX() < toX) robot.stepForward();

        }
        else if (robot.getX() > toX) {

            while (robot.getDirection() != Direction.LEFT) robot.turnRight();

            while (robot.getX() > toX) robot.stepForward();
        }

    }

    public final class ComplexNumber {
        private final double re;
        private final double im;

        public ComplexNumber(double re, double im) {
            this.re = re;
            this.im = im;
        }

        public double getRe() {
            return re;
        }

        public double getIm() {
            return im;
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;
            ComplexNumber that = (ComplexNumber) o;
            return Double.compare(that.re, re) == 0 &&
                    Double.compare(that.im, im) == 0;
        }

        @Override
        public int hashCode() {
            return Objects.hash(re, im);
        }
    }

    public static double integrate(DoubleUnaryOperator f, double a, double b) {
      int n=(int)((b - a)*10e6);
        double  h = 1e-6;
        double S=0;

        for(int i = 0; i < n-1; i++)
        {
          double  x = a + i * h;
            S += f.applyAsDouble(x);
        }
        S = h * S;

        return S;
    }

    public static int mid(int v){
        return (v/10)%1000;
    }
    public static IntStream pseudoRandomStream(int seed) {
       return IntStream.iterate(seed,s->mid(s*s));
    }

    public static String readAsString(InputStream inputStream, Charset charset) throws IOException {

        InputStreamReader fs=new InputStreamReader(inputStream,charset);
        int s=fs.read();
        StringBuilder b=new StringBuilder();
        while (s>=0){
            b.append((char) s);
            s = fs.read();
        }

        return new String(b);
    }
    public static InputStream getStream(byte [] data)  {
        return new ByteArrayInputStream (data);
    }
    public static int checkSumOfStream(InputStream inputStream) throws IOException {
        InputStreamReader fs=new InputStreamReader(inputStream);
        int C = 0;
        int n;

        while ((n = inputStream.read()) != -1) {
            C = Integer.rotateLeft(C,1)^n;
        }

        return C;
    }


    public static <T, U> Function<T, U> ternaryOperator(
            Predicate<? super T> condition,
            Function<? super T, ? extends U> ifTrue,
            Function<? super T, ? extends U> ifFalse) {
        return (t)->{
            return condition.test(t)?
                ifTrue.apply(t):
              ifFalse.apply(t);
        };

    }

    public static <T> Set<T> symmetricDifference(Set<? extends T> set1, Set<? extends T> set2) {

        Set<T> s1 = new HashSet<T>(set1);
        Set<T> s2 = new HashSet<T>(set2);
        Set<T> s3 = new HashSet<T>(s1);
        s1.removeAll(s2);
        s2.removeAll(s3);
        s1.addAll(s2);

        return s1;


    }

    public class AsciiCharSequence implements java.lang.CharSequence {

        byte[] arr;
        public AsciiCharSequence(byte[] bt){
            arr=Arrays.copyOf(bt,bt.length);
        }

        /**
         * Returns the length of this character sequence.  The length is the number
         * of 16-bit {@code char}s in the sequence.
         *
         * @return the number of {@code char}s in this sequence
         */
        @Override
        public int length() {
            return arr.length;
        }

        /**
         * Returns the {@code char} value at the specified index.  An index ranges from zero
         * to {@code length() - 1}.  The first {@code char} value of the sequence is at
         * index zero, the next at index one, and so on, as for array
         * indexing.
         *
         * <p>If the {@code char} value specified by the index is a
         * <a href="{@docRoot}/java.base/java/lang/Character.html#unicode">surrogate</a>, the surrogate
         * value is returned.
         *
         * @param index the index of the {@code char} value to be returned
         * @return the specified {@code char} value
         * @throws IndexOutOfBoundsException if the {@code index} argument is negative or not less than
         *                                   {@code length()}
         */
        @Override
        public char charAt(int index) {
            return (char)arr[index];
        }

        /**
         * Returns a {@code CharSequence} that is a subsequence of this sequence.
         * The subsequence starts with the {@code char} value at the specified index and
         * ends with the {@code char} value at index {@code end - 1}.  The length
         * (in {@code char}s) of the
         * returned sequence is {@code end - start}, so if {@code start == end}
         * then an empty sequence is returned.
         *
         * @param start the start index, inclusive
         * @param end   the end index, exclusive
         * @return the specified subsequence
         * @throws IndexOutOfBoundsException if {@code start} or {@code end} are negative,
         *                                   if {@code end} is greater than {@code length()},
         *                                   or if {@code start} is greater than {@code end}
         */
        @Override
        public AsciiCharSequence subSequence(int start, int end) {
            return new AsciiCharSequence(Arrays.copyOfRange(arr,start,end));
        }

        @Override
        public String toString(){
            char[] m=new char[arr.length];
            for(int i=0;i<arr.length;i++)
                m[i]=(char)arr[i];
            return String.copyValueOf(m);
        }

    }

    public static double sqrt(double x) {
        if(x<0) throw new  java.lang.IllegalArgumentException("Expected non-negative number, got ?");
        return Math.sqrt(x); // your implementation here
    }

    public static <T> void findMinMax(
            Stream<? extends T> stream,
            Comparator<? super T> order,
            BiConsumer<? super T, ? super T> minMaxConsumer) {

        T[] arr=(T[])stream.sorted(order).toArray();
        if(arr.length==0)
            minMaxConsumer.accept(null, null);
        else minMaxConsumer.accept(arr[0], arr[arr.length-1]);

    }


}



